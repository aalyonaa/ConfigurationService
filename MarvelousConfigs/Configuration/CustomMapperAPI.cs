using AutoMapper;
using Marvelous.Contracts.ResponseModels;
using MarvelousConfigs.API.Models;
using MarvelousConfigs.BLL.Models;

namespace MarvelousConfigs.API.Configuration
{
    public class CustomMapperAPI : Profile
    {
        public CustomMapperAPI()
        {
            CreateMap<ConfigInputModel, ConfigModel>();
            CreateMap<ConfigModel, ConfigOutputModel>();
            CreateMap<ConfigModel, ConfigResponseModel>();

            CreateMap<MicroserviceInputModel, MicroserviceModel>();
            CreateMap<MicroserviceModel, MicroserviceOutputModel>();

            CreateMap<MicroserviceWithConfigsModel, MicroserviceWithConfigsOutputModel>()
                .ForMember(x => x.Configs, opt => opt.MapFrom(o => o.Configs));
        }
    }
}
