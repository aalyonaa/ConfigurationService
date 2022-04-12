using System.ComponentModel.DataAnnotations;

namespace MarvelousConfigs.API.Models
{
    public class ConfigInputModel
    {
        [Required]
        public string Value { get; set; }
        public string Description { get; set; }
    }
}
