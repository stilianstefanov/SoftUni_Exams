namespace VaporStore.DataProcessor
{
    using Data;
    using Newtonsoft.Json;
    using System.Globalization;
    using System.Text;
    using System.Xml.Serialization;
    using VaporStore.Data.Models;
    using VaporStore.DataProcessor.ExportDto;

    public static class Serializer
    {
        public static string ExportGamesByGenres(VaporStoreDbContext context, string[] genreNames)
        {
            var GenresAndGames = context.Genres
                .Where(gr => genreNames.Contains(gr.Name))
                .ToArray()
                .Select(gr => new
                {
                    Id = gr.Id,
                    Genre = gr.Name,
                    Games = gr.Games.Where(g => g.Purchases.Any())
                                    .Select(g => new
                                    {
                                        Id = g.Id,
                                        Title = g.Name,
                                        Developer = g.Developer.Name,
                                        Tags = string.Join(", ", g.GameTags.Select(gt => gt.Tag.Name)),
                                        Players = g.Purchases.Count
                                    })
                                    .OrderByDescending(g => g.Players)
                                    .ThenBy(g => g.Id)
                                    .ToArray(),
                    TotalPlayers = gr.Games.Where(g => g.Purchases.Any()).Sum(g => g.Purchases.Count)
                })
                .OrderByDescending(gr => gr.TotalPlayers)
                .ThenBy(gr => gr.Id)
                .ToArray();

            return JsonConvert.SerializeObject(GenresAndGames, Formatting.Indented);
        }

        public static string ExportUserPurchasesByType(VaporStoreDbContext context, string purchaseType)
        {
            ExportUserDto[] userDtos = context
                .Users
                .ToArray()
                .Where(u => u.Cards.Any(c => c.Purchases.Any(p => p.Type.ToString() == purchaseType)))
                .Select(u => new ExportUserDto()
                {
                    Username = u.Username,
                    TotalSpent = u.Cards
                        .Sum(c => c.Purchases
                        .Where(p => p.Type.ToString() == purchaseType)
                        .Sum(p => p.Game.Price)),
                    Purchases = u.Cards
                        .SelectMany(c => c.Purchases
                        .Where(p => p.Type.ToString() == purchaseType)
                        .Select(p => new ExportPurchaseDto()
                        {
                            Card = p.Card.Number,
                            Cvc = p.Card.Cvc,
                            Date = p.Date.ToString("yyyy-MM-dd HH:mm", CultureInfo.InvariantCulture),
                            Game = new ExportGameDto()
                            {
                                Genre = p.Game.Genre.Name,
                                Title = p.Game.Name,
                                Price = p.Game.Price
                            }
                        }))
                        .OrderBy(p => p.Date)
                        .ToArray()
                })
                .OrderByDescending(u => u.TotalSpent)
                .ThenBy(u => u.Username)
                .ToArray();

            return Serialize<ExportUserDto[]>(userDtos, "Users");
        }

        private static string Serialize<T>(T obj, string rootName)
        {
            StringBuilder sb = new StringBuilder();

            XmlRootAttribute xmlRoot =
                new XmlRootAttribute(rootName);
            XmlSerializer xmlSerializer =
                new XmlSerializer(typeof(T), xmlRoot);

            XmlSerializerNamespaces namespaces = new XmlSerializerNamespaces();
            namespaces.Add(string.Empty, string.Empty);

            using StringWriter writer = new StringWriter(sb);
            xmlSerializer.Serialize(writer, obj, namespaces);

            return sb.ToString().TrimEnd();
        }
    }
}