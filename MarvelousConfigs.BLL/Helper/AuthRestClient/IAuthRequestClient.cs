using Marvelous.Contracts.RequestModels;
using Marvelous.Contracts.ResponseModels;
using RestSharp;

namespace MarvelousConfigs.BLL.AuthRequestClient
{
    public interface IAuthRequestClient
    {
        Task<bool> GetRestResponse(string token);
        Task<RestResponse> GetToken(AuthRequestModel auth);
        Task<RestResponse<IdentityResponseModel>> SendRequestToValidateToken(string jwtToken);
    }
}