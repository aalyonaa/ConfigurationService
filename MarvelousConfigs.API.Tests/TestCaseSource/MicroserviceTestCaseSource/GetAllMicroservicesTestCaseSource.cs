using Marvelous.Contracts.Enums;
using Marvelous.Contracts.ResponseModels;
using MarvelousConfigs.BLL.Models;
using System.Collections;
using System.Collections.Generic;

namespace MarvelousConfigs.API.Tests
{
    public class GetAllMicroservicesTestCaseSource : IEnumerable
    {
        public IEnumerator GetEnumerator()
        {
            List<MicroserviceModel> services = new List<MicroserviceModel>() { new MicroserviceModel()
            {
               Id = 1,
               ServiceName = "Test1",
               Url = "https://test1",
               Address = "123456"
            } };
            IdentityResponseModel model = new IdentityResponseModel()
            {
                Id = 1,
                Role = "Admin",
                IssuerMicroservice = Microservice.MarvelousConfigs.ToString()
            };

            yield return new object[] { services, model };
        }
    }
}