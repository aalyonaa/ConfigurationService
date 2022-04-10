using MarvelousConfigs.DAL.Entities;
using System.Collections;

namespace MarvelousConfig.BLL.Tests
{
    public class DeleteOrRestoreConfigTestCaseSource : IEnumerable
    {
        public IEnumerator GetEnumerator()
        {
            Config config = new Config()
            {
                Id = 1,
                Key = "KEY",
                Value = "VALUE"
            };

            yield return new object[] { config };
        }
    }
}