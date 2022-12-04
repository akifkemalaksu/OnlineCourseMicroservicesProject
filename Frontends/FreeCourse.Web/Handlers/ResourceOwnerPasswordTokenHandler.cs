using FreeCourse.Shared.Dtos;
using FreeCourse.Web.Exceptions;
using FreeCourse.Web.Models.Baskets;
using FreeCourse.Web.Services.Interfaces;
using Microsoft.AspNetCore.Authentication;
using Microsoft.IdentityModel.Protocols.OpenIdConnect;
using System.Net;

namespace FreeCourse.Web.Handlers
{
    public class ResourceOwnerPasswordTokenHandler : DelegatingHandler
    {
        private readonly IHttpContextAccessor _httpContextAccessor;
        private readonly IIdentityService _identityService;
        private readonly ILogger<ResourceOwnerPasswordTokenHandler> _logger;

        public ResourceOwnerPasswordTokenHandler(IHttpContextAccessor httpContextAccessor, IIdentityService identityService, ILogger<ResourceOwnerPasswordTokenHandler> logger)
        {
            _httpContextAccessor = httpContextAccessor;
            _identityService = identityService;
            _logger = logger;
        }

        protected override async Task<HttpResponseMessage> SendAsync(HttpRequestMessage request, CancellationToken cancellationToken)
        {
            var accessToken = await _httpContextAccessor.HttpContext.GetTokenAsync(OpenIdConnectParameterNames.AccessToken);

            request.Headers.Authorization = new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", accessToken);

            var response = await base.SendAsync(request, cancellationToken);

            if (response.StatusCode == System.Net.HttpStatusCode.Unauthorized)
            {
                var tokenResponse = await _identityService.GetAccessTokenByRefreshToken();

                if (tokenResponse != null)
                {
                    request.Headers.Authorization = new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", tokenResponse.AccessToken);

                    response = await base.SendAsync(request, cancellationToken);
                }
            }

            if (response.StatusCode == System.Net.HttpStatusCode.Unauthorized)
            {
                throw new UnAuthorizeException();
            }

            if (response.StatusCode == HttpStatusCode.NotFound)
            {
                return await HandleNotFoundResponse(response);
            }

            return response;
        }

        private Task<HttpResponseMessage> HandleNotFoundResponse(HttpResponseMessage response)
        {
            var responseContentAsJson = response.Content.ReadFromJsonAsync<Response<dynamic>>();
            if (responseContentAsJson.Result is not null)
            {
                response = new HttpResponseMessage();
                response.StatusCode = HttpStatusCode.OK;
                response.Content = JsonContent.Create(responseContentAsJson.Result);
            }

            return Task.FromResult(response);
        }
    }
}