namespace Boardgames.DataProcessor
{ 
    using Newtonsoft.Json;
    using System.Text;
    using System.Xml.Serialization;

    using Boardgames.Data;
    using Boardgames.DataProcessor.ExportDto;

    public class Serializer
    {
        public static string ExportCreatorsWithTheirBoardgames(BoardgamesContext context)
        {
            ExportCreatorDto[] creators = context.Creators
                .Where(c => c.Boardgames.Any())
                .ToArray()
                .Select(c => new ExportCreatorDto()
                {
                    CreatorName = c.FirstName + " " + c.LastName,
                    BoardgamesCount = c.Boardgames.Count,
                    Boardgames = c.Boardgames.Select(b => new ExportBoardgameDto()
                    {
                        BoardgameName = b.Name,
                        BoardgameYearPublished = b.YearPublished
                    })
                    .OrderBy(b => b.BoardgameName)
                    .ToArray()
                })
                .OrderByDescending(c => c.Boardgames.Count())
                .ThenBy(c => c.CreatorName)
                .ToArray();

            return Serialize<ExportCreatorDto[]>(creators, "Creators");
        }

        public static string ExportSellersWithMostBoardgames(BoardgamesContext context, int year, double rating)
        {
            var sellers = context.Sellers
                .Where(s => s.BoardgamesSellers.Any(bs => bs.Boardgame.YearPublished >= year && bs.Boardgame.Rating <= rating))
                .ToArray()
                .Select(s => new
                {
                    Name = s.Name,
                    Website = s.Website,
                    Boardgames = s.BoardgamesSellers.Where(bs => bs.Boardgame.YearPublished >= year && bs.Boardgame.Rating <= rating)
                                                    .Select(bs => new
                                                    {
                                                        Name = bs.Boardgame.Name,
                                                        Rating = bs.Boardgame.Rating,
                                                        Mechanics = bs.Boardgame.Mechanics,
                                                        Category = bs.Boardgame.CategoryType.ToString()
                                                    })
                                                    .OrderByDescending(b => b.Rating)
                                                    .ThenBy(b => b.Name)
                                                    .ToArray()
                })
                .OrderByDescending(s => s.Boardgames.Count())
                .ThenBy(s => s.Name)
                .Take(5)
                .ToArray();

            return JsonConvert.SerializeObject(sellers, Formatting.Indented);
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