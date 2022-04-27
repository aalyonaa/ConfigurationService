using Marvelous.Contracts.RequestModels;
using System.Collections;

namespace MarvelousConfigs.API.Tests
{
    internal class LoginTestCaseSource : IEnumerable
    {
        public IEnumerator GetEnumerator()
        {
            AuthRequestModel auth = new AuthRequestModel()
            {
                Email = "test",
                Password = "test"
            };

            yield return new object[] { auth };
        }
    }
}