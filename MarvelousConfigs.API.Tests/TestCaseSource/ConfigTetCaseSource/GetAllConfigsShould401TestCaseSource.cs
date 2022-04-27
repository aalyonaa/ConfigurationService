using Marvelous.Contracts.Enums;
using Marvelous.Contracts.ResponseModels;
using MarvelousConfigs.BLL.Models;
using System.Collections;
using System.Collections.Generic;

namespace MarvelousConfigs.API.Tests
{
    internal class GetAllConfigsShould401TestCaseSource : IEnumerable
    {
        public IEnumerator GetEnumerator()
        {
            List<ConfigModel> services = new List<ConfigModel>() { new ConfigModel()
            {
                Id = 1,
                Key = "Key1",
                Value = "Value1",
                ServiceId = 1,
                Created = System.DateTime.Now
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