using Marvelous.Contracts.RequestModels;
using Marvelous.Contracts.ResponseModels;

namespace MarvelousConfigs.BLL.Infrastructure
{
    public interface IAuthRequestClient
    {
        Task SendRequestWithToken(string token);
        Task<string> GetToken(AuthRequestModel auth);
        Task<IdentityResponseModel> SendRequestToValidateToken(string jwtToken);
    }
}