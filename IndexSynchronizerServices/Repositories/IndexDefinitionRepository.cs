using IndexSynchronizerServices.Models;
using Microsoft.Data.SqlClient;

using System.Text;

namespace IndexSynchronizerServices.Repositories
{
	public class IndexDefinitionRepository : IIndexDefinitionRepository
	{
		// TODO: this is not actually async; make-it-so.gif
		// TODO: inject datastore context/provider/something or build DAL instead doing the very silly thing of building connection strings on the fly
		public async Task<IEnumerable<String>> GetIndexDefinitionsAsync(IConnectionDetails connectionDetails)
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

			// TODO: embedded resource? something else? decisions, decisions
			var query = File.ReadAllText("Scripts/GetIndexDefinitions.sql");

			// TODO: make this strongly typed; e.g. List<IndexDefinition> where IndexDefinition has members including but not limited to:
			// key columns, included columns, column order, asc/desc, etc.
			// You'd think there would be a library for this already, but I can't seem to find one
			var definitions = new List<String>();
			
			using (var connection = new SqlConnection(builder.ToString()))
			using (var command = new SqlCommand(query, connection))
			{
				command.Parameters.AddWithValue("@TableName", connectionDetails.TableName);

				connection.Open();

				using(var reader = command.ExecuteReader())
				{
					
					while (reader.Read())
					{
						definitions.Add(reader.GetString(0));
					}
				}

				connection.Close();
			}

			return definitions;
		}
	}
}
