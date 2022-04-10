namespace MarvelousConfigs.API.Models
{
    public class MicroserviceWithConfigsOutputModel
    {
        public int Id { get; set; }
        public string ServiceName { get; set; }
        public string Url { get; set; }
        public string Address { get; set; }
        public List<ConfigOutputModel> Configs { get; set; }

    }
}
