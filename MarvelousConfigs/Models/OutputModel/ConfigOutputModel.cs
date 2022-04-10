namespace MarvelousConfigs.API.Models
{
    public class ConfigOutputModel
    {
        public string Key { get; set; }
        public string Value { get; set; }
        public string Description { get; set; }
        public DateTime Created { get; set; }
        public DateTime? Updated { get; set; }
        public int ServiceId { get; set; }
    }
}
