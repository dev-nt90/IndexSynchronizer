using IndexSynchronizer.Models;
using Microsoft.Data.SqlClient;

using System.Text;

namespace IndexSynchronizer.Repositories
{
	public class IndexPreviewRepository : IIndexPreviewRepository
	{
		public async Task<IEnumerable<String>> GetIndexDefinitionsAsync(IConnectionDetails connectionDetails)
		{
			var connectionString = 
				$"Server={connectionDetails.ServerName};" +
				$"Database={connectionDetails.DatabaseName};" +
				$"User Id={connectionDetails.Username};" +
				$"Password={connectionDetails.Password};" +
				$"TrustServerCertificate=true"; // TODO: extra spicy - remove this and do the real work of certs

			var query = File.ReadAllText("Scripts/GetIndexDefinitions.sql");

			var definitions = new List<String>();
			
			// TODO: make async
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
