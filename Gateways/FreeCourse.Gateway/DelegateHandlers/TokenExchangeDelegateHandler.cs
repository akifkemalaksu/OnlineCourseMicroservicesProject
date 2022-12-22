using FreeCourse.Gateway.Models;
using IdentityModel.Client;
using Microsoft.Extensions.Options;

namespace FreeCourse.Gateway.DelegateHandlers
{
    public class TokenExchangeDelegateHandler : DelegatingHandler
    {
        private readonly HttpClient _httpClient;
        private readonly IConfiguration _configuration;
        private readonly TokenExchangeSettings _tokenExchangeSettings;

        private string _accessToken;

        public TokenExchangeDelegateHandler(HttpClient httpClient, IConfiguration configuration, IOptions<TokenExchangeSettings> tokenExchangeSettingsOptions)
        {
            _httpClient = httpClient;
            _configuration = configuration;
            _tokenExchangeSettings = tokenExchangeSettingsOptions.Value;
        }

        private async Task<string> GetToken(string requestToken)
        {
            if (!string.IsNullOrEmpty(_accessToken))
                return _accessToken;

            var discoveryEndPoints = await _httpClient.GetDiscoveryDocumentAsync(new DiscoveryDocumentRequest()
            {
                Address = _configuration["IdentityServerUrl"],
                Policy = new DiscoveryPolicy() { RequireHttps = false }
            });

            if (discoveryEndPoints.IsError)
                throw discoveryEndPoints.Exception;

            TokenExchangeTokenRequest tokenExchangeTokenRequest = new TokenExchangeTokenRequest()
            {
                Address = discoveryEndPoints.TokenEndpoint,
                ClientId = _tokenExchangeSettings.ClientId,
                ClientSecret = _tokenExchangeSettings.ClientSecret,
                GrantType = _tokenExchangeSettings.TokenGrantType,
                SubjectToken = requestToken,
                SubjectTokenType = _tokenExchangeSettings.SubjectTokenType,
                Scope = _tokenExchangeSettings.Scope
            };

            var tokenResponse = await _httpClient.RequestTokenExchangeTokenAsync(tokenExchangeTokenRequest);
            if (tokenResponse.IsError)
                throw tokenResponse.Exception;

            _accessToken = tokenResponse.AccessToken;

            return _accessToken;
        }

        protected override async Task<HttpResponseMessage> SendAsync(HttpRequestMessage request, CancellationToken cancellationToken)
        {
            var requestToken = request.Headers.Authorization.Parameter;

            var newToken = await GetToken(requestToken);

            request.SetBearerToken(newToken);

            return await base.SendAsync(request, cancellationToken);
        }
    }
}
