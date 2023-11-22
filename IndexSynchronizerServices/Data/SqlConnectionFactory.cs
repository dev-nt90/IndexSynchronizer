using IndexSynchronizerServices.Models;
using Microsoft.Data.SqlClient;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IndexSynchronizerServices.Data
{
	public static class SqlConnectionFactory
	{
		public static SqlConnection Create(IConnectionDetails connectionDetails)
		{
			var builder = new SqlConnectionStringBuilder
			{
				DataSource = connectionDetails.ServerName,
				InitialCatalog = connectionDetails.DatabaseName,
				UserID = connectionDetails.Username,
				Password = connectionDetails.Password,

				// TODO: as a toy project not ever leaving localhost this is fine, but if this
				// ever makes it to production for some misguided reason, remove this and do the real work of certs
				TrustServerCertificate = true
			};
			return new SqlConnection(builder.ToString());
		}
	}
}
