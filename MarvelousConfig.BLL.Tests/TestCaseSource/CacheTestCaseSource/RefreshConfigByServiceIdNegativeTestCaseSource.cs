using MarvelousConfigs.DAL.Entities;
using System.Collections;
using System.Collections.Generic;

namespace MarvelousConfigs.BLL.Tests
{
    internal class RefreshConfigByServiceIdNegativeTestCaseSource : IEnumerable
    {
        public IEnumerator GetEnumerator()
        {
            List<Config> configs = new List<Config>() {
            new Config()
            {
                Id = 1,
                Key = "KEY",
                Value = "VALUE",
                ServiceId = 1
            },
            new Config()
            {
                Id = 2,
                Key = "KEY",
                Value = "VALUE",
            },
            new Config()
            {
                Key = "KEY",
                Value = "VALUE",
                ServiceId = 2
            }};

            Microservice service = new Microservice()
            {
                Id = 3,
                Url = "URL1"
            };

            int id = 3;

            yield return new object[] { id, configs, service };
        }
    }
}