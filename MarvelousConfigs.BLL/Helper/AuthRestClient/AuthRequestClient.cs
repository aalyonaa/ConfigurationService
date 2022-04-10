using Marvelous.Contracts.Autentificator;
using Marvelous.Contracts.Endpoints;
using Marvelous.Contracts.Enums;
using Microsoft.Extensions.Logging;
using RestSharp;

namespace MarvelousConfigs.BLL.AuthRequestClient
{
    public class AuthRequestClient : IAuthRequestClient
    {
        private readonly ILogger<AuthRequestClient> _logger;
        private readonly string _url = "https://piter-education.ru:6042";

        public AuthRequestClient(ILogger<AuthRequestClient> logger)
        {
            _logger = logger;
        }

        public async Task<bool> GetRestResponse(string token)
        {
            _logger.LogInformation($"Start sending a request to validate a token");
            bool response = false;
            try
            {
                response = await SendRequestWithToken(token);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"Error while validating token");
            }

            _logger.LogInformation($"Response from {Microservice.MarvelousAuth} was received");
            return response;
        }

        private async Task<bool> SendRequestWithToken(string token)
        {
            var client = new RestClient(new RestClientOptions(_url)
            {
                Timeout = 300000
            });
            client.Authenticator = new MarvelousAuthenticator(token);
            client.AddDefaultHeader(nameof(Microservice), value: Microservice.MarvelousConfigs.ToString());
            var request = new RestRequest($"{AuthEndpoints.ApiAuth}{AuthEndpoints.ValidationMicroservice}", Method.Get);
            _logger.LogInformation($"Getting a response from {Microservice.MarvelousAuth}");
            var response = await client.ExecuteAsync(request);
            return CheckTransactionError(response);
        }


        private bool CheckTransactionError(RestResponse response)
        {
            bool result = false;
            if (response.StatusCode != System.Net.HttpStatusCode.OK)
            {
                _logger.LogWarning(response.ErrorException!.Message);
            }
            else if (response.StatusCode == System.Net.HttpStatusCode.OK)
            {
                result = true;
            }
            if (response.Content == null)
            {
                _logger.LogWarning(response.ErrorException!.Message);
            }
            return result;
        }
    }
}
