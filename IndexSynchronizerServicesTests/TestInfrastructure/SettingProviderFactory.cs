using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using IndexSynchronizerServicesTests.TestInfrastructure.Config;
using Microsoft.Data.SqlClient;
using Microsoft.Extensions.Configuration;


namespace IndexSynchronizerServicesTests.TestInfrastructure
{
    /// <summary>
    /// Factory for creating settings providers. Settings providers do what they say - they provide domain-specific settings for callers.
    /// </summary>
    public sealed class SettingProviderFactory
    {
        private IConfigurationRoot? configRoot;
        
        public SettingProviderFactory()
        {
            this.configRoot = this.GetConfiguration();
		}

        // TODO: break this out as it increases in complexity
        public ConfigPoco CreateConfig()
        {
            var newConfig = new ConfigPoco();
            var sqlServerSection = this.configRoot!.GetSection("TestConnections:SqlServer");
            sqlServerSection.Bind(newConfig);
            return newConfig;
		}
		
        private IConfigurationRoot? GetConfiguration()
		{
			return new ConfigurationBuilder().AddJsonFile("appsettings.json").Build();
		}
	}
}
