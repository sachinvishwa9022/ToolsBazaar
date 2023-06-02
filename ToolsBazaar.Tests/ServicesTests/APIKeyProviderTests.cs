using FluentAssertions;
using Microsoft.Extensions.Configuration;
using NSubstitute;
using ToolsBazaar.Web.Models;
using ToolsBazaar.Web.Services;

namespace ToolsBazaar.Tests.ServicesTests
{
    public class APIKeyProviderTests
    {
        private IConfiguration _configuration;
        private IConfigurationSection _configurationSection;
        public APIKeyProviderTests()
        {
            _configurationSection = Substitute.For<IConfigurationSection>();
            _configuration = Substitute.For<IConfiguration>();
        }

        [Fact]
        public void GivenKeyInParameter_WhenMatchedWithValueInSettings_ThenShouldReturnAPIKey()
        {

            string key = "1234";
            string appSettingKey = "AppSettings:APIKey";

            _configurationSection.Value.Returns(key);
            _configuration.GetSection(appSettingKey).Returns(_configurationSection);

            APIKeyProvider apiKeyProvider = new APIKeyProvider(_configuration);

            var returnValue = apiKeyProvider.ProvideAsync(key);

            returnValue.Result.Should().NotBeNull().And.BeOfType(typeof(APIKey));
            returnValue.Result.Key.Should().BeEquivalentTo(key, "Match the api key");

            _configuration.Received().GetSection(appSettingKey);
        }

        [Fact]
        public void GivenKeyInParameter_WhenValueNotMatchedWithSettingKey_ThenShouldNotReturnAPIKey()
        {
            string key = "1234";
            string passedKey = "1111";
            string appSettingKey = "AppSettings:APIKey";

            _configurationSection.Value.Returns(key);
            _configuration.GetSection(appSettingKey).Returns(_configurationSection);

            APIKeyProvider apiKeyProvider = new APIKeyProvider(_configuration);

            var returnValue = apiKeyProvider.ProvideAsync(passedKey);

            returnValue.Result.Should().BeNull();
            _configuration.Received().GetSection(appSettingKey);

        }
    }
}
