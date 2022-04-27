using MarvelousConfigs.DAL.Entities;
using System.Collections;
using System.Collections.Generic;

namespace MarvelousConfigs.BLL.Tests
{
    internal class SetMemoryCacheNegativeTestCaseSource : IEnumerable
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

            List<Microservice> services = new List<Microservice>() {
            new Microservice()
            {
                Id = 14567890,
                Url = "URL1"
            },
            new Microservice()
            {
                ServiceName = "Name2",
                Url = "URL2"
            },
            new Microservice()
            {
                Id = 3,
                ServiceName = "Name3",
            }};

            yield return new object[] { configs, services };
        }
    }
}