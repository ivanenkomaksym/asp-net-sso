using Microsoft.IdentityModel.Protocols.OpenIdConnect;
using System.Text;
using Xunit;
using Xunit.Abstractions;

namespace IdentityServer.IntegrationTests
{
    public class IdentityServerIntegrationTests(ITestOutputHelper Output)
    {
        [Fact]
        public async Task Can_Get_Token_From_IdentityServer()
        {
            // Arramge
            using var application = new ApplicationBase(Output);
            var client = application.CreateClient();

            // Simulate the token request as curl would do
            var requestBody = new StringContent("client_id=client&client_secret=secret&grant_type=client_credentials&scope=api1",
                                                Encoding.UTF8,
                                                "application/x-www-form-urlencoded");

            // Act
            var response = await client.PostAsync("/connect/token", requestBody);

            // Assert
            response.EnsureSuccessStatusCode(); // Ensure the response is a success.

            var content = await response.Content.ReadAsStringAsync();
            var message = new OpenIdConnectMessage(content);

            Assert.Equal("Bearer", message.TokenType);
            Assert.Equal("api1", message.Scope);
            Assert.NotEmpty(message.AccessToken);
        }
    }
}