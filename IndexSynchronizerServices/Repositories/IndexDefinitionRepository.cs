using IndexSynchronizerServices.Models;
using Microsoft.Data.SqlClient;

using System.Text;

namespace IndexSynchronizerServices.Repositories
{
	public class IndexDefinitionRepository : IIndexDefinitionRepository
	{
		// TODO: this is not actually async
		// make-it-so.gif
		public async Task<IEnumerable<String>> GetIndexDefinitionsAsync(IConnectionDetails connectionDetails)
		{
			var connectionString = 
				$"Server={connectionDetails.ServerName};" +
				$"Database={connectionDetails.DatabaseName};" +
				$"User Id={connectionDetails.Username};" +
				$"Password={connectionDetails.Password};" +
				$"TrustServerCertificate=true"; // TODO: extra spicy - remove this and do the real work of certs

			var query = File.ReadAllText("Scripts/GetIndexDefinitions.sql");

			// TODO: make this strongly typed; e.g. List<IndexDefinition> where IndexDefinition has members including but not limited to:
			// key columns, included columns, column order, asc/desc, etc.
			// There's _probably_ already a library or type for this. Will _probably_ have to extend on a per-supported-database-and-version basis
			var definitions = new List<String>();
			
			using (var connection = new SqlConnection(connectionString))
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
