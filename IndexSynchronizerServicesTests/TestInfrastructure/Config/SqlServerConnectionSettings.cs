using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IndexSynchronizerServicesTests.TestInfrastructure.Config
{
	public class SqlServerConnectionSettings
	{
		public String MasterDatabaseConnectionString { get; set; }
		public String ServerName { get; set; }
		public String DatabaseName { get; set; }
		public String UserName { get; set; }
		public String Password { get; set; }
	}
}
