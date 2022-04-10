using System.ComponentModel.DataAnnotations;

namespace MarvelousConfigs.API.Models
{
    public class ConfigInputModel
    {
        [Required]
        public string Key { get; set; }
        [Required]
        public string Value { get; set; }
        public string Description { get; set; }
        [Required]
        [Range(1, 10)]
        public int ServiceId { get; set; }
    }
}
