using Marvelous.Contracts.Enums;
using Marvelous.Contracts.ResponseModels;
using MarvelousConfigs.API.Models;
using System.Collections;

namespace MarvelousConfigs.API.Tests
{
    internal class UpdateConfigShould401TestCaseSource : IEnumerable
    {
        public IEnumerator GetEnumerator()
        {
            ConfigInputModel services = new ConfigInputModel()
            {
                Value = "Value1",
            };

            IdentityResponseModel model = new IdentityResponseModel()
            {
                Id = 1,
                Role = "Admin",
                IssuerMicroservice = Microservice.MarvelousConfigs.ToString()
            };

            int id = 1;

            yield return new object[] { services, model, id };
        }
    }
}