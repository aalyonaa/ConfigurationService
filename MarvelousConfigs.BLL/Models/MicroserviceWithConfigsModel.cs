namespace MarvelousConfigs.BLL.Models
{
    public class MicroserviceWithConfigsModel
    {
        public int Id { get; set; }
        public string ServiceName { get; set; }
        public string Url { get; set; }
        public string Address { get; set; }
        public List<ConfigModel> Configs { get; set; }
    }
}
