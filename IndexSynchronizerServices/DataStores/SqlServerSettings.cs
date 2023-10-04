using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IndexSynchronizerServices.DataStores
{
	public sealed class SqlServerSettings : DatabaseSettings
	{
		public SqlServerSettings(String connectionString, SqlServerDatabaseOptions sqlServerDatabaseOptions = null)
			: base(connectionString)
		{
			ConnectionString = connectionString;
		}
	}
}
