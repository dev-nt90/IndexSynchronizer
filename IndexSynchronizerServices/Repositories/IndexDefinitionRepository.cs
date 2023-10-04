using IndexSynchronizerServices.Models;
using Microsoft.Data.SqlClient;

using System.Text;

namespace IndexSynchronizerServices.Repositories
{
	public class IndexDefinitionRepository : IIndexDefinitionRepository
	{
		// TODO: this is not actually async; make-it-so.gif
		// TODO: inject datastore context/provider/something instead of building a connection string on the fly
		public async Task<IEnumerable<String>> GetIndexDefinitionsAsync(IConnectionDetails connectionDetails)
		{
			var connectionString = 
				$"Server={connectionDetails.ServerName};" +
				$"Database={connectionDetails.DatabaseName};" +
				$"User Id={connectionDetails.Username};" +
				$"Password={connectionDetails.Password};" +
				$"TrustServerCertificate=true"; // TODO: extra spicy - remove this and do the real work of certs

			// TODO: embedded resource? something else? decisions, decisions
			var query = File.ReadAllText("Scripts/GetIndexDefinitions.sql");

			// TODO: make this strongly typed; e.g. List<IndexDefinition> where IndexDefinition has members including but not limited to:
			// key columns, included columns, column order, asc/desc, etc.
			// You'd think there would be a library for this already, but I can't seem to find one
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
