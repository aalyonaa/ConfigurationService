using Marvelous.Contracts.Enums;
using Marvelous.Contracts.ResponseModels;
using MarvelousConfigs.API.Models;
using System.Collections;

namespace MarvelousConfigs.API.Tests
{
    public class UpdateMicroserviceShould401TestCaseSource : IEnumerable
    {
        public IEnumerator GetEnumerator()
        {
            MicroserviceInputModel services = new MicroserviceInputModel()
            {
                Url = "https://test1",
                Address = "123456"
            };

            IdentityResponseModel model = new IdentityResponseModel()
            {
                Id = 1,
                Role = "Vip",
                IssuerMicroservice = Microservice.MarvelousConfigs.ToString()
            };

            int id = 1;

            yield return new object[] { services, model, id };
        }
    }
}