using Marvelous.Contracts.RequestModels;
using System.Collections;

namespace MarvelousConfigs.BLL.Tests
{
    public class GetTokenTestCaseSource : IEnumerable
    {
        public IEnumerator GetEnumerator()
        {
            AuthRequestModel auth = new AuthRequestModel()
            {
                Email = "Test",
                Password = "Test"
            };

            yield return new object[] { auth };
        }
    }
}