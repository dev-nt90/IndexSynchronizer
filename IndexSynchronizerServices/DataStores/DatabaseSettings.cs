using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IndexSynchronizerServices.DataStores
{
	public abstract class DatabaseSettings
	{
		public DatabaseSettings(string connectionString)
		{
			this.ConnectionString = connectionString;
		}

		public string ConnectionString { get; set; }
	}
}
