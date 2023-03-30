namespace VaporStore.DataProcessor
{
    using System.ComponentModel.DataAnnotations;
    using System.Globalization;
    using System.Text;
    using System.Xml.Serialization;
    using Data;
    using Newtonsoft.Json;
    using VaporStore.Data.Models;
    using VaporStore.Data.Models.Enums;
    using VaporStore.DataProcessor.ImportDto;

    public static class Deserializer
    {
        public const string ErrorMessage = "Invalid Data";

        public const string SuccessfullyImportedGame = "Added {0} ({1}) with {2} tags";

        public const string SuccessfullyImportedUser = "Imported {0} with {1} cards";

        public const string SuccessfullyImportedPurchase = "Imported {0} for {1}";

        public static string ImportGames(VaporStoreDbContext context, string jsonString)
        {
            var sb = new StringBuilder();

            ImportGameDto[] gameDtos = JsonConvert.DeserializeObject<ImportGameDto[]>(jsonString)!;

            ICollection<Game> validGames = new List<Game>();

            ICollection<Developer> developers = new List<Developer>();
            ICollection<Genre> genres = new List<Genre>();
            ICollection<Tag> tags = new List<Tag>();

            foreach (var gameDto in gameDtos)
            {
                if (gameDto.Price < 0 || string.IsNullOrEmpty(gameDto.Name)
                    || string.IsNullOrEmpty(gameDto.ReleaseDate) || string.IsNullOrEmpty(gameDto.Developer)
                    || string.IsNullOrEmpty(gameDto.Genre))
                {
                    sb.AppendLine(ErrorMessage);
                    continue;
                }

                if (gameDto.Tags == null || !gameDto.Tags.Any())
                {
                    sb.AppendLine(ErrorMessage);
                    continue;
                }

                bool isReleaseDateValid = DateTime.TryParseExact(gameDto.ReleaseDate, "yyyy-MM-dd", 
                    CultureInfo.InvariantCulture, DateTimeStyles.None, out DateTime releaseDate);
                if (!isReleaseDateValid)
                {
                    sb.AppendLine(ErrorMessage);
                    continue;
                }

                Developer? developer = developers.FirstOrDefault(d => d.Name == gameDto.Developer);
                Genre? genre = genres.FirstOrDefault(g => g.Name == gameDto.Genre);

                if (developer == null)
                {
                    developer = new Developer()
                    {
                        Name = gameDto.Developer
                    };

                    developers.Add(developer);
                }
                if (genre == null)
                {
                    genre = new Genre()
                    {
                        Name = gameDto.Genre
                    };
                    genres.Add(genre);
                }

                Game game = new Game()
                {
                    Name = gameDto.Name,
                    Price = gameDto.Price,
                    ReleaseDate = releaseDate,
                    Developer = developer,
                    Genre = genre
                };

                foreach (var tagName in gameDto.Tags)
                {
                    if (string.IsNullOrEmpty(tagName))
                    {
                        continue;
                    }

                    Tag? tag = tags.FirstOrDefault(t => t.Name == tagName);

                    if (tag == null)
                    {
                        tag = new Tag()
                        {
                            Name = tagName
                        };
                        tags.Add(tag);
                    }

                    game.GameTags.Add(new GameTag()
                    {
                        Tag = tag,
                        Game = game
                    });
                }

                validGames.Add(game);
                sb.AppendLine(string.Format(SuccessfullyImportedGame, game.Name, game.Genre.Name, game.GameTags.Count));
            }

            context.Games.AddRange(validGames);
            context.SaveChanges();

            return sb.ToString().TrimEnd();
        }

        public static string ImportUsers(VaporStoreDbContext context, string jsonString)
        {
            var sb = new StringBuilder();

            ImportUserDto[] userDtos = JsonConvert.DeserializeObject<ImportUserDto[]>(jsonString)!;

            ICollection<User> validUsers = new HashSet<User>();
            foreach (var userDto in userDtos)
            {
                if (!IsValid(userDto) || string.IsNullOrEmpty(userDto.Email)
                    || userDto.Cards == null || !userDto.Cards.Any())
                {
                    sb.AppendLine(ErrorMessage);
                    continue;
                }

                if (userDto.Cards.Any(c => !IsValid(c)))
                {
                    sb.AppendLine(ErrorMessage);
                    continue;
                }

                User user = new User()
                {
                    FullName = userDto.FullName!,
                    Username = userDto.Username!,
                    Email = userDto.Email!,
                    Age = userDto.Age
                };

                bool isValidCardDto = true;

                foreach (var cardDto in userDto.Cards)
                {
                    bool isCardTypeValid = Enum.TryParse<CardType>(cardDto.Type, out CardType cardType);
                    if (!isCardTypeValid)
                    {
                        isValidCardDto = false;
                        sb.AppendLine(ErrorMessage);
                        break;
                    }

                    user.Cards.Add(new Card()
                    {
                        Number = cardDto.Number!,
                        Cvc = cardDto.Cvc!,
                        Type = cardType
                    });
                }

                if (!isValidCardDto)
                {
                    continue;
                }

                validUsers.Add(user);
                sb.AppendLine(string.Format(SuccessfullyImportedUser, user.Username, user.Cards.Count));
            }

            context.Users.AddRange(validUsers);
            context.SaveChanges();

            return sb.ToString().TrimEnd();
        }

        public static string ImportPurchases(VaporStoreDbContext context, string xmlString)
        {
            var sb = new StringBuilder();
            ImportPurchaseDto[] purchaseDtos = Deserialize<ImportPurchaseDto[]>(xmlString, "Purchases");

            ICollection<Purchase> validPurchases = new HashSet<Purchase>();
            foreach (var purchaseDto in purchaseDtos)
            {
                if (!IsValid(purchaseDto))
                {
                    sb.AppendLine(ErrorMessage);
                    continue;
                }

                Game? game = context.Games.FirstOrDefault(g => g.Name == purchaseDto.GameTitle);
                Card? card = context.Cards.FirstOrDefault(c => c.Number == purchaseDto.CardNumber);

                if (game == null || card == null)
                {
                    sb.AppendLine(ErrorMessage);
                    continue;
                }

                bool isTypeValid = Enum.TryParse<PurchaseType>(purchaseDto.Type, out PurchaseType type);

                bool isDateValid = DateTime.TryParseExact(purchaseDto.Date, "dd/MM/yyyy HH:mm",
                    CultureInfo.InvariantCulture, DateTimeStyles.None, out  DateTime date);

                if (!isTypeValid || !isDateValid)
                {
                    sb.AppendLine(ErrorMessage);
                    continue;
                }

                Purchase purchase = new Purchase()
                {
                    Type = type,
                    ProductKey = purchaseDto.ProductKey!,
                    Date = date,
                    Game = game,
                    Card = card
                };

                validPurchases.Add(purchase);
                sb.AppendLine(string.Format(SuccessfullyImportedPurchase, game.Name, card.User.Username));
            }

            context.Purchases.AddRange(validPurchases);
            context.SaveChanges();

            return sb.ToString().TrimEnd();
        }

        private static bool IsValid(object dto)
        {
            var validationContext = new ValidationContext(dto);
            var validationResult = new List<ValidationResult>();

            return Validator.TryValidateObject(dto, validationContext, validationResult, true);
        }

        private static T Deserialize<T>(string inputXml, string rootName)
        {
            XmlRootAttribute xmlRoot = new XmlRootAttribute(rootName);
            XmlSerializer xmlSerializer =
                new XmlSerializer(typeof(T), xmlRoot);

            using StringReader reader = new StringReader(inputXml);
            T deserializedDtos =
                (T)xmlSerializer.Deserialize(reader);

            return deserializedDtos;
        }
    }
}