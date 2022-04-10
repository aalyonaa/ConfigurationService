using AutoMapper;
using MarvelousConfigs.BLL.Models;
using MarvelousConfigs.DAL;
using MarvelousConfigs.DAL.Entities;

namespace MarvelousConfigs.BLL.Configuration
{
    public class CustomMapperBLL : Profile
    {
        public CustomMapperBLL()
        {
            CreateMap<Microservice, MicroserviceModel>().ReverseMap();
            CreateMap<Config, ConfigModel>().ReverseMap();
            CreateMap<MicroserviceWithConfigs, MicroserviceWithConfigsModel>()
                .ForMember(x => x.Configs, z => z.MapFrom(c => c.Configs)).ReverseMap();
        }
    }
}
