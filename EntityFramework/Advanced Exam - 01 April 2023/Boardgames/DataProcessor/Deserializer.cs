namespace Boardgames.DataProcessor
{
    using System.ComponentModel.DataAnnotations;
    using System.Text;
    using System.Xml.Serialization;
    using Newtonsoft.Json;

    using Data;
    using Data.Models;
    using Data.Models.Enums;
    using ImportDto;


    public class Deserializer
    {
        private const string ErrorMessage = "Invalid data!";

        private const string SuccessfullyImportedCreator
            = "Successfully imported creator – {0} {1} with {2} boardgames.";

        private const string SuccessfullyImportedSeller
            = "Successfully imported seller - {0} with {1} boardgames.";

        public static string ImportCreators(BoardgamesContext context, string xmlString)
        {
            var sb = new StringBuilder();

            ImportCreatorDto[] creatorDtos = Deserialize<ImportCreatorDto[]>(xmlString, "Creators");

            ICollection<Creator> validCreators = new HashSet<Creator>();
            foreach (var creatorDto in creatorDtos)
            {
                if (!IsValid(creatorDto))
                {
                    sb.AppendLine(ErrorMessage);
                    continue;
                }

                Creator creator = new Creator()
                {
                    FirstName = creatorDto.FirstName,
                    LastName = creatorDto.LastName
                };

                foreach (var boardGameDto in creatorDto.Boardgames)
                {
                    if (!IsValid(boardGameDto))
                    {
                        sb.AppendLine(ErrorMessage);
                        continue;
                    }

                    creator.Boardgames.Add(new Boardgame()
                    {
                        Name = boardGameDto.Name,
                        Rating = boardGameDto.Rating,
                        YearPublished = boardGameDto.YearPublished,
                        CategoryType = (CategoryType)boardGameDto.CategoryType,
                        Mechanics = boardGameDto.Mechanics
                    });
                }

                validCreators.Add(creator);
                sb.AppendLine(string.Format(SuccessfullyImportedCreator, creator.FirstName, creator.LastName, creator.Boardgames.Count));
            }

            context.Creators.AddRange(validCreators);
            context.SaveChanges();

            return sb.ToString().TrimEnd();
        }

        public static string ImportSellers(BoardgamesContext context, string jsonString)
        {
            var sb = new StringBuilder();

            ImportSellerDto[] sellerDtos = JsonConvert.DeserializeObject<ImportSellerDto[]>(jsonString)!;

            ICollection<Seller> validSellers = new HashSet<Seller>();
            foreach (var sellerDto in sellerDtos)
            {
                if (!IsValid(sellerDto))
                {
                    sb.AppendLine(ErrorMessage);
                    continue;
                }

                Seller seller = new Seller()
                {
                    Name = sellerDto.Name,
                    Address = sellerDto.Address,
                    Country = sellerDto.Country,
                    Website = sellerDto.Website
                };

                foreach (var boardGameid in sellerDto.Boardgames.Distinct())
                {
                    if (!context.Boardgames.Any(b => b.Id == boardGameid))
                    {
                        sb.AppendLine(ErrorMessage);
                        continue;
                    }

                    seller.BoardgamesSellers.Add(new BoardgameSeller()
                    {
                        BoardgameId = boardGameid,
                        Seller = seller
                    });
                }

                validSellers.Add(seller);
                sb.AppendLine(string.Format(SuccessfullyImportedSeller, seller.Name, seller.BoardgamesSellers.Count));
            }

            context.Sellers.AddRange(validSellers);
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
