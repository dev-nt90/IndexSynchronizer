using IndexSynchronizerServices.Data;
using IndexSynchronizerServices.Models;
using Microsoft.Data.SqlClient;

namespace IndexSynchronizerServices.Repositories
{
	public class IndexDefinitionRepository : IIndexDefinitionRepository
	{
		// TODO: inject datastore context/provider/something or build DAL instead doing the very silly thing of building connection strings on the fly
		public async Task<IDictionary<String, String>> GetIndexDefinitionsAsync(IConnectionDetails connectionDetails)
		{
			// TODO: the below script is too complicated to embed directly in code
			// so... embedded resource? something else? decisions, decisions
			var query = File.ReadAllText("Scripts/GetIndexDefinitions.sql");
			var definitions = new Dictionary<String, String>();
			
			using (var connection = SqlConnectionFactory.Create(connectionDetails))
			using (var command = new SqlCommand(query, connection))
			{
				command.Parameters.AddWithValue("@TableName", connectionDetails.TableName);
				command.Parameters.AddWithValue("@SchemaName", connectionDetails.SchemaName);

				connection.Open();

				using(var reader = await command.ExecuteReaderAsync())
				{
					while (await reader.ReadAsync())
					{
						// key: index name, value: index definition
						definitions.Add(reader.GetString(1), reader.GetString(0));
					}
				}

				connection.Close();
			}

			return definitions;
		}
	}
}
