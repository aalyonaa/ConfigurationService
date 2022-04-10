using System.ComponentModel.DataAnnotations;

namespace MarvelousConfigs.API.Models
{
    public class MicroserviceInputModel
    {
        [Required]
        public string ServiceName { get; set; }
        [Required]
        public string Url { get; set; }
        [Required]
        public string Address { get; set; }
    }
}
