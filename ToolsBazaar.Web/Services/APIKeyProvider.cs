using AspNetCore.Authentication.ApiKey;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Options;
using ToolsBazaar.Web.Models;

namespace ToolsBazaar.Web.Services
{
    public class APIKeyProvider : IApiKeyProvider
    {
        private readonly IConfiguration _configuration;
        public APIKeyProvider(IConfiguration configuration)
        {
            _configuration = configuration;
        }

        public Task<IApiKey> ProvideAsync(string key)
        {

            IApiKey apiKey = null;
            var APIKeyValue = _configuration.GetValue<string>("AppSettings:APIKey");

            if (APIKeyValue.Equals(key, StringComparison.OrdinalIgnoreCase))
            {
                apiKey = new APIKey(key, "requestor");
            }

            return Task.FromResult(apiKey);

        }
    }
}
