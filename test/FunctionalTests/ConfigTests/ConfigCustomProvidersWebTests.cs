using System.Threading.Tasks;
using Microsoft.AspNetCore.Server.Testing;
using Microsoft.AspNetCore.Testing.xunit;
using Xunit;

namespace EntropyTests.ConfigTests
{
    public class ConfigCustomProvidersWebTests
    {
        private const string SiteName = "Config.CustomProviders.Web";

        [Theory]
        [InlineData(ServerType.Kestrel, RuntimeFlavor.CoreClr, RuntimeArchitecture.x64, "http://localhost:5750")]
        public async Task RunSite_AllPlatforms(ServerType server, RuntimeFlavor runtimeFlavor, RuntimeArchitecture architecture, string applicationBaseUrl)
        {
            await RunSite(server, runtimeFlavor, architecture, applicationBaseUrl);
        }

        [ConditionalTheory]
        [OSSkipCondition(OperatingSystems.Linux)]
        [OSSkipCondition(OperatingSystems.MacOSX)]
        //[InlineData(ServerType.WebListener, RuntimeFlavor.Clr, RuntimeArchitecture.x86, "http://localhost:5751")]
        [InlineData(ServerType.WebListener, RuntimeFlavor.Clr, RuntimeArchitecture.x64, "http://localhost:5752")]
        //[InlineData(ServerType.WebListener, RuntimeFlavor.CoreClr, RuntimeArchitecture.x86, "http://localhost:5753")]
        [InlineData(ServerType.WebListener, RuntimeFlavor.CoreClr, RuntimeArchitecture.x64, "http://localhost:5754")]
        //[InlineData(ServerType.Kestrel, RuntimeFlavor.Clr, RuntimeArchitecture.x86, "http://localhost:5755")]
        [InlineData(ServerType.Kestrel, RuntimeFlavor.Clr, RuntimeArchitecture.x64, "http://localhost:5756")]
        //[InlineData(ServerType.Kestrel, RuntimeFlavor.CoreClr, RuntimeArchitecture.x86, "http://localhost:5757")]
        // Already covered by all platforms:
        //[InlineData(ServerType.WebListener, RuntimeFlavor.CoreClr, RuntimeArchitecture.x86, "http://localhost:5758")]
        public async Task RunSite_WindowsOnly(ServerType server, RuntimeFlavor runtimeFlavor, RuntimeArchitecture architecture, string applicationBaseUrl)
        {
            await RunSite(server, runtimeFlavor, architecture, applicationBaseUrl);
        }

        [ConditionalTheory]
        [OSSkipCondition(OperatingSystems.Windows)]
        [InlineData(ServerType.Kestrel, RuntimeFlavor.Clr, RuntimeArchitecture.x64, "http://localhost:5759")]
        [InlineData(ServerType.Kestrel, RuntimeFlavor.CoreClr, RuntimeArchitecture.x64, "http://localhost:5760")]
        public async Task RunSite_NonWindowsOnly(ServerType server, RuntimeFlavor runtimeFlavor, RuntimeArchitecture architecture, string applicationBaseUrl)
        {
            await RunSite(server, runtimeFlavor, architecture, applicationBaseUrl);
        }

        private async Task RunSite(ServerType serverType, RuntimeFlavor runtimeFlavor, RuntimeArchitecture architecture, string applicationBaseUrl)
        {
            await TestServices.RunSiteTest(
                SiteName,
                serverType,
                runtimeFlavor,
                architecture,
                applicationBaseUrl,
                async (httpClient, logger, token) =>
                {
                    var response = await RetryHelper.RetryRequest(async () =>
                    {
                        return await httpClient.GetAsync(string.Empty);
                    }, logger, token, retryCount: 30);

                    var responseText = await response.Content.ReadAsStringAsync();
                    string expectedText =
"[Hardcoded] \r\n" +
"  [1] \r\n" +
"    [Caption] One\r\n" +
"  [2] \r\n" +
"    [Caption] Two\r\n";

                    logger.LogResponseOnFailedAssert(response, responseText, () =>
                    {
                        Assert.Equal(expectedText, responseText);
                    });
                });
        }
    }
}
