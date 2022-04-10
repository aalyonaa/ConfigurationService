using MarvelousConfigs.BLL.Models;
using MarvelousConfigs.DAL.Entities;
using System.Collections;

namespace MarvelousConfigs.BLL.Tests
{
    public class UpdateMicroserviceByIdTestCaseSource : IEnumerable
    {
        public IEnumerator GetEnumerator()
        {
            Microservice service = new Microservice()
            {
                Id = 1,
                ServiceName = "Name1",
                Url = "URL1"
            };

            MicroserviceModel model = new MicroserviceModel()
            {
                Id = 1,
                ServiceName = "Name1",
                Url = "URL1"
            };

            int id = 1;

            yield return new object[] { id, service, model };
        }
    }
}