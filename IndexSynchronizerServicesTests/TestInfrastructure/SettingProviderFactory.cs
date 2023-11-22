using Microsoft.Data.SqlClient;
using Microsoft.Extensions.Configuration;
using System.Diagnostics;

namespace IndexSynchronizerServicesTests.TestInfrastructure
{
    /// <summary>
    /// Factory for creating settings providers. Settings providers do what they say - they provide domain-specific settings for callers.
    /// </summary>
    public sealed class SettingProviderFactory
    {
        private readonly IConfiguration _configuration;

        public SettingProviderFactory()
        {
            _configuration = new ConfigurationBuilder().AddEnvironmentVariables().Build();
        }

        public IConfiguration Configuration { get { return _configuration; } }

        public String BuildDatabaseUnderTestConnectionString()
        {
            var envConStr = Environment.GetEnvironmentVariable("MasterTestConnectionString") + ";TrustServerCertificate=true;Encrypt=False";

            return envConStr.Replace("master", "AdventureWorks"); // TODO: get db name from config
        }

        public String BuildMasterDbConnectionString()
        {
            return Environment.GetEnvironmentVariable("MasterTestConnectionString") + ";TrustServerCertificate=true;Encrypt=False";
        }
    }
}
