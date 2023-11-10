using Microsoft.Data.SqlClient;
using Microsoft.Extensions.Configuration;

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
			var database = "AdventureWorks"; // TODO: from config
			var envIp = Environment.GetEnvironmentVariable("CONTAINER_IP");
			var server = "localhost";
			var user = "IndexSyncTestLogin"; // TODO: from env
			var pwd = "ChangeThisHardc0dedThing!"; // TODO: from env

			var sqlStringBuilder = new SqlConnectionStringBuilder
			{
				DataSource = server,
				InitialCatalog = database,
				UserID = user,
				Password = pwd,

				// TODO: as a toy project not ever leaving localhost this is fine, but if this
				// ever makes it to production for some misguided reason, remove this and do the real work of certs
				TrustServerCertificate = true
			};

			return sqlStringBuilder.ToString();
		}

		public String BuildMasterDbConnectionString()
		{
			var envIp = Environment.GetEnvironmentVariable("CONTAINER_IP");
			var server = "localhost";
			var database = "master"; // TODO: from config
			var user = "sa"; // TODO: from env
			var pwd = "ChangeThisHardc0dedThing!"; // TODO: from env

			var sqlStringBuilder = new SqlConnectionStringBuilder
			{
				DataSource = server,
				InitialCatalog = database,
				UserID = user,
				Password = pwd,

				// TODO: as a toy project not ever leaving localhost this is fine, but if this
				// ever makes it to production for some misguided reason, remove this and do the real work of certs
				TrustServerCertificate = true
			};

			return sqlStringBuilder.ToString();
		}
	}
}
