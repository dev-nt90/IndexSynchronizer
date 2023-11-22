using IndexSynchronizerServices.Data;
using IndexSynchronizerServices.Models;
using Microsoft.Data.SqlClient;
using System.Text;

namespace IndexSynchronizerServices.Repositories
{
	public class IndexSyncRepository : IIndexSyncRepository
	{
		public async Task DoIndexSync(IDictionary<String, String> sourceIndexDefinitions, IConnectionDetails target)
		{
			var queryBuilder = new StringBuilder();

			queryBuilder.AppendLine("BEGIN TRAN ");
			
			foreach(var kvp in sourceIndexDefinitions)
			{
				queryBuilder.Append($" DROP INDEX IF EXISTS {kvp.Key} ON {target.SchemaName}.{target.TableName}; ");
			}

			queryBuilder.AppendLine("COMMIT TRAN ");

			queryBuilder.AppendLine("BEGIN TRAN ");

			foreach (var kvp in sourceIndexDefinitions)
			{
				queryBuilder.Append($" {kvp.Value} ");
			}

			queryBuilder.AppendLine("COMMIT TRAN ");

			using (var connection = SqlConnectionFactory.Create(target))
			using(var command = new SqlCommand(queryBuilder.ToString(), connection))
			{
				connection.Open();
				await command.ExecuteNonQueryAsync();
				connection.Close();
			}
		}

	}
}
