using System.ComponentModel.DataAnnotations;

namespace MarvelousConfigs.API.Models
{
    public class MicroserviceInputModel
    {
        [Required]
        [Url]
        public string Url { get; set; }
        [Required]
        public string Address { get; set; }
    }
}
