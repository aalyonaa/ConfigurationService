using Marvelous.Contracts.Enums;
using MarvelousConfigs.BLL.Models;
using System.Collections;
using System.Collections.Generic;

namespace MarvelousConfigs.API.Tests
{
    internal class GetConfigsByServiceTestCaseSource : IEnumerable
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

            Microservice microservice = Microservice.MarvelousAuth;

            yield return new object[] { services, microservice };
        }
    }
}