namespace SoftJail.DataProcessor.ImportDto
{
    using Newtonsoft.Json;
    using System.ComponentModel.DataAnnotations;

    public class ImportDepartmentDto
    {
        [JsonProperty("Name")]
        [MinLength(ValidationConstants.DepartmentNameMinLength)]
        [MaxLength(ValidationConstants.DepartmentNameMaxLength)]
        [Required]
        public string Name { get; set; } = null!;

        [JsonProperty("Cells")]
        public ImportCellDto[]? Cells { get; set; }
    }
}
