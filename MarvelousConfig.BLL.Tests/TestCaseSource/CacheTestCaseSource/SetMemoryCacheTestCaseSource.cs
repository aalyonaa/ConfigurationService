using MarvelousConfigs.DAL.Entities;
using System.Collections;
using System.Collections.Generic;

namespace MarvelousConfigs.BLL.Tests
{
    internal class SetMemoryCacheTestCaseSource : IEnumerable
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
                ServiceId = 1
            },
            new Config()
            {
                Id = 3,
                Key = "KEY",
                Value = "VALUE",
                ServiceId = 2
            }};

            List<Microservice> services = new List<Microservice>() {
            new Microservice()
            {
                Id = 1,
                ServiceName = "Name1",
                Url = "URL1"
            },
            new Microservice()
            {
                Id = 2,
                ServiceName = "Name2",
                Url = "URL2"
            },
            new Microservice()
            {
                Id = 3,
                ServiceName = "Name3",
                Url = "URL3"
            }};

            yield return new object[] { configs, services };
        }
    }
}