namespace Theatre.DataProcessor
{
    using Newtonsoft.Json;
    using System.ComponentModel.DataAnnotations;
    using System.Globalization;
    using System.Text;
    using System.Xml.Serialization;
    using Theatre.Data;
    using Theatre.Data.Models;
    using Theatre.Data.Models.Enums;
    using Theatre.DataProcessor.ImportDto;

    public class Deserializer
    {
        private const string ErrorMessage = "Invalid data!";

        private const string SuccessfulImportPlay
            = "Successfully imported {0} with genre {1} and a rating of {2}!";

        private const string SuccessfulImportActor
            = "Successfully imported actor {0} as a {1} character!";

        private const string SuccessfulImportTheatre
            = "Successfully imported theatre {0} with #{1} tickets!";



        public static string ImportPlays(TheatreContext context, string xmlString)
        {
            var sb = new StringBuilder();

            ImportPlayDto[] playDtos = Deserialize<ImportPlayDto[]>(xmlString, "Plays");

            ICollection<Play> validPlays = new HashSet<Play>();
            foreach (var playDto in playDtos)
            {
                if (!IsValid(playDto) || string.IsNullOrEmpty(playDto.Description))
                {
                    sb.AppendLine(ErrorMessage);
                    continue;
                }

                bool isDurationValid = TimeSpan.TryParseExact(playDto.Duration, "c", CultureInfo.InvariantCulture, out TimeSpan duration);
                bool IsGenreValid = Enum.TryParse<Genre>(playDto.Genre, out Genre genre);

                if (!IsGenreValid || !isDurationValid)
                {
                    sb.AppendLine(ErrorMessage);
                    continue;
                }

                if (duration.Hours < 1)
                {
                    sb.AppendLine(ErrorMessage);
                    continue;
                }

                Play play = new Play()
                {
                    Title = playDto.Title!,
                    Duration = duration,
                    Genre = genre,
                    Rating = (float)playDto.Rating,
                    Description = playDto.Description!,
                    Screenwriter = playDto.Screenwriter!
                };

                validPlays.Add(play);
                sb.AppendLine(string.Format(SuccessfulImportPlay, play.Title, play.Genre.ToString(), play.Rating));
            }

            context.AddRange(validPlays);
            context.SaveChanges();

            return sb.ToString().TrimEnd();
        }

        public static string ImportCasts(TheatreContext context, string xmlString)
        {
            var sb = new StringBuilder();

            ImportCastDto[] castDtos = Deserialize<ImportCastDto[]>(xmlString, "Casts");

            ICollection<Cast> validCasts = new HashSet<Cast>();
            foreach (var dto in castDtos)
            {
                if (!IsValid(dto))
                {
                    sb.AppendLine(ErrorMessage);
                    continue;
                }

                Cast cast = new Cast()
                {
                    FullName = dto.FullName!,
                    IsMainCharacter = dto.IsMainCharacter,
                    PhoneNumber = dto.PhoneNumber!,
                    PlayId = dto.PlayId
                };

                validCasts.Add(cast);
                sb.AppendLine(string.Format(SuccessfulImportActor, cast.FullName, cast.IsMainCharacter ? "main" : "lesser"));
            }

            context.AddRange(validCasts);
            context.SaveChanges();

            return sb.ToString().TrimEnd();
        }

        public static string ImportTtheatersTickets(TheatreContext context, string jsonString)
        {
            var sb = new StringBuilder();

            ImportTheatreDto[] theatreDtos = JsonConvert.DeserializeObject<ImportTheatreDto[]>(jsonString)!;

            ICollection<Theatre> validTheatres = new HashSet<Theatre>();
            foreach (var theatreDto in theatreDtos)
            {
                if (!IsValid(theatreDto))
                {
                    sb.AppendLine(ErrorMessage);
                    continue;
                }

                var theatre = new Theatre()
                {
                    Name = theatreDto.Name!,
                    NumberOfHalls = (sbyte)theatreDto.NumberOfHalls,
                    Director = theatreDto.Director!
                };

                foreach (var ticketDto in theatreDto.Tickets)
                {
                    if (!IsValid(ticketDto))
                    {
                        sb.AppendLine(ErrorMessage);
                        continue;
                    }

                    theatre.Tickets.Add(new Ticket()
                    {
                        Price = ticketDto.Price,
                        RowNumber = (sbyte)ticketDto.RowNumber,
                        PlayId = ticketDto.PlayId,
                        Theatre = theatre
                    });
                }

                validTheatres.Add(theatre);
                sb.AppendLine(string.Format(SuccessfulImportTheatre, theatre.Name, theatre.Tickets.Count));
            }

            context.AddRange(validTheatres);
            context.SaveChanges();

            return sb.ToString().TrimEnd();
        }


        private static bool IsValid(object obj)
        {
            var validator = new ValidationContext(obj);
            var validationRes = new List<ValidationResult>();

            var result = Validator.TryValidateObject(obj, validator, validationRes, true);
            return result;
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
