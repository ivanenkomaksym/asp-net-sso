using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.Extensions.Configuration.Memory;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Xunit.Abstractions;

namespace IdentityServer.IntegrationTests
{
    internal class ApplicationBase : WebApplicationFactory<Program>
    {
        private readonly ITestOutputHelper Output;
        public ApplicationBase(ITestOutputHelper output)
        {
            Output = output;
        }

        protected virtual IEnumerable<KeyValuePair<string, string>> GetMemoryConfiguration()
        {
            return [];
        }

        protected override IHost CreateHost(IHostBuilder builder)
        {
            builder.ConfigureAppConfiguration(configurationBuilder =>
            {
                var configuration = GetMemoryConfiguration();
                if (configuration == null)
                    return;

                configurationBuilder.Sources.Clear();
                var fromMemory = new MemoryConfigurationSource { InitialData = configuration };
                configurationBuilder.Add(fromMemory);
            }).ConfigureServices(services =>
            {
                services.AddControllers().AddApplicationPart(typeof(Program).Assembly);
            });

            return base.CreateHost(builder);
        }
    }
}
