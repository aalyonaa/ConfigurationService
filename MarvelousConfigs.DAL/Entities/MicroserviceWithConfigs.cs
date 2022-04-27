using MarvelousConfigs.DAL.Entities;

namespace MarvelousConfigs.DAL
{
    public class MicroserviceWithConfigs
    {
        public int Id { get; set; }
        public string ServiceName { get; set; }
        public string Url { get; set; }
        public string Address { get; set; }
        public List<Config> Configs { get; set; }
    }
}
