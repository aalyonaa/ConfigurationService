using MarvelousConfigs.BLL.Models;
using MarvelousConfigs.DAL.Entities;
using System.Collections;

namespace MarvelousConfig.BLL.Tests
{
    public class UpdateConfigByIdTestCaseSource : IEnumerable
    {
        public IEnumerator GetEnumerator()
        {
            Config config = new Config()
            {
                Id = 1,
                Key = "KEY",
                Value = "VALUE",
                //Created = System.DateTime.Now
            };

            ConfigModel model = new ConfigModel()
            {
                Id = 1,
                Key = "KEY",
                Value = "VALUE",
                // Created = System.DateTime.Now
            };

            int id = 1;

            yield return new object[] { id, config, model };
        }
    }
}