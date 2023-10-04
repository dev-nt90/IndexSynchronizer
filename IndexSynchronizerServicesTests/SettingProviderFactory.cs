using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using IndexSynchronizerServices.DataStores;

namespace IndexSynchronizerServicesTests
{
	/// <summary>
	/// Factory for creating settings providers. Settings providers do what they say - they provide domain-specific settings for callers.
	/// </summary>
	public sealed class SettingProviderFactory
	{
		public IDataStoreSettingProvider CreateDataStoreSettingProvider()
		{
			// key: setting name; value: setting value
			// TODO: fetch source/target databases from appsettings
			// TODO: fetch test runner credentials from registry
			var dataStoreSettings = new Dictionary<String, String>
			{
				{ "sqlServerSourceConnectionString", "server=TODO;user id=TODO;password=TODO;initial catalog=TODO" }
			};

			throw new NotImplementedException();
		}
	}
}
