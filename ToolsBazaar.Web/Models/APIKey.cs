using AspNetCore.Authentication.ApiKey;
using System.Security.Claims;

namespace ToolsBazaar.Web.Models
{
    public class APIKey : IApiKey
    {
        public APIKey(string key, string owner, List<Claim> claims = null)
        {
            Key = key;
            OwnerName = owner;
            Claims = claims ?? new List<Claim>();
        }

        public string Key { get; }
        public string OwnerName { get; }
        public IReadOnlyCollection<Claim> Claims { get; }
    }
}
