
using Marvelous.Contracts.RequestModels;

namespace MarvelousConfigs.BLL.Services
{
    public interface IAuthService
    {
        string GetToken(AuthRequestModel auth);
    }
}