using MarvelousConfigs.BLL.Models;
using System.Collections;

namespace MarvelousConfig.BLL.Tests
{
    public class AddConfigTestCaseSource : IEnumerable
    {
        public IEnumerator GetEnumerator()
        {
            ConfigModel model = new ConfigModel()
            {
                Key = "KEY",
                Value = "VALUE"
            };

            yield return new object[] { model };
        }
    }
}