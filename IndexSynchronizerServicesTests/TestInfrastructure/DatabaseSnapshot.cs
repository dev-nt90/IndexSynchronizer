using Microsoft.Data.SqlClient;

using System.Data;
using System.Text;

namespace IndexSynchronizerServicesTests.TestInfrastructure
{
	public class DatabaseSnapshot
	{
		private readonly String databaseName;
		private readonly String snapshotName;
		private readonly String masterDatabaseConnectionString;

		public DatabaseSnapshot(String databaseName, String masterDatabaseConnectionString)
		{
			this.databaseName = databaseName;
			this.snapshotName = String.Concat(databaseName, "_SS");
			this.masterDatabaseConnectionString = masterDatabaseConnectionString;
		}

		public void Take(Boolean overwriteExisting = true)
		{
			if (this.DoesDatabaseSnapshotExist())
			{
				if (overwriteExisting)
				{
					this.DropDatabaseSnapshot();
				}
				else
				{
					return;
				}
			}

			using var sqlConnection = new SqlConnection(this.masterDatabaseConnectionString);
			using var command = new SqlCommand();
			command.Connection = sqlConnection;
			command.CommandType = CommandType.Text;

			// First get the files for this db.
			command.CommandText = $@"
                SELECT
                    Name = name,
                    FileName = physical_name
                FROM {this.databaseName}.sys.database_files
                WHERE type = 0
                    ORDER BY file_id;";

			sqlConnection.Open();

			var sqlBuilder = new StringBuilder();

			sqlBuilder.AppendLine(
				$@"USE master;

                CREATE DATABASE {this.snapshotName}
                    ON
                ");

			var files = new List<String>();

			using (var reader = command.ExecuteReader())
			{
				while (reader.Read())
				{
					var fileLogicalName = (String)reader["Name"];
					var filePhysicalName = (String)reader["FileName"];

					files.Add($"( NAME = {fileLogicalName}, FILENAME = '{filePhysicalName}.SS' )");
				}
			}

			sqlBuilder.AppendLine(String.Join(",", files));

			sqlBuilder.AppendLine($"AS SNAPSHOT OF {this.databaseName};");

			/* Now create the snapshot, 
			 * e.g
				CREATE DATABASE AdventureWorks_SS
				ON
				( NAME = AdventureWorks_Primary, FILENAME = 'C:\Program Files\Microsoft SQL Server\MSSQL15.SQLSERVER2019\MSSQL\DATA\AdventureWorks_Primary.SS' ),
				( NAME = AdventureWorks_Data, FILENAME = 'C:\Program Files\Microsoft SQL Server\MSSQL15.SQLSERVER2019\MSSQL\DATA\AdventureWorks_Data.SS' ),
				( NAME = AdventureWorks_Index, FILENAME = 'C:\Program Files\Microsoft SQL Server\MSSQL15.SQLSERVER2019\MSSQL\DATA\AdventureWorks_Index.SS' )
				AS SNAPSHOT OF AdventureWorks;
			*/
			command.CommandText = sqlBuilder.ToString();
			command.ExecuteScalar();
		}

		private Boolean DoesDatabaseSnapshotExist()
		{
			var doesDatabaseExist = false;
			using (var cnn = new SqlConnection(this.masterDatabaseConnectionString))
			{
				using (IDbCommand cmd = cnn.CreateCommand())
				{
					cmd.Connection = cnn;
					cmd.CommandTimeout = 1000;
					cmd.CommandText = String.Format("SELECT COUNT(*) FROM sys.databases WHERE name = '{0}'", this.snapshotName);
					cnn.Open();

					if ((Int32)cmd.ExecuteScalar() == 1)
					{
						doesDatabaseExist = true;
					}

					return doesDatabaseExist;
				}
			}
		}

		public void DropDatabaseSnapshot()
		{
			using (var cnn = new SqlConnection(this.masterDatabaseConnectionString))
			{
				using (var cmd = cnn.CreateCommand())
				{
					String commandText = 		
						$@"
						USE master;

						DECLARE @SnapshotName NVARCHAR(MAX);
                        DECLARE @cmd NVARCHAR(MAX);

                        DECLARE SnapshotNameCursor CURSOR LOCAL FAST_FORWARD
                        FOR
                        SELECT name 
                        FROM sys.databases 
                        WHERE source_database_id=DB_ID('{this.snapshotName}')

                        OPEN SnapshotNameCursor

                        FETCH NEXT FROM SnapshotNameCursor
                        INTO @SnapshotName;

                        WHILE(@@FETCH_STATUS = 0)
                        BEGIN
                            SET @cmd = CONCAT('DROP DATABASE IF EXISTS ', @SnapshotName);
                            EXEC(@cmd);

                            FETCH NEXT FROM SnapshotNameCursor
                            INTO @SnapshotName;
                        END;

                        CLOSE SnapshotNameCursor;
                        DEALLOCATE SnapshotNameCursor;
                            
                        DROP DATABASE IF EXISTS {this.snapshotName};";

					cmd.Connection = cnn;
					cmd.CommandTimeout = 1000;
					cmd.CommandText = commandText;
					cnn.Open();
					cmd.ExecuteNonQuery();
				}
			}
		}

		public Boolean RestoreFromSnapshot()
		{
			using (var cnn = new SqlConnection(this.masterDatabaseConnectionString))
			{
				cnn.Open();

				var serviceBrokerEnabled = (Boolean)this.DetermineIfServiceBrokerEnabled(cnn);

				this.SetRestrictedUserMode(cnn);

				var success = this.RestoreFromSnapshot(cnn);

				// service broker has to be explicitly re-enabled for activation queues to get created right.
				if (serviceBrokerEnabled)
				{
					this.SetErrorBrokerConversations(cnn);
				}

				this.SetMultiUserMode(cnn);
				cnn.Close();

				ClearConnections();

				return success;
			}
		}

		/// <summary>
		/// Clears all database connections.
		/// </summary>
		private void ClearConnections()
		{
			// Using just ClearPool won't hit the connections used by LINQ, so clear all pools.
			SqlConnection.ClearAllPools();
		}

		/// <summary>
		/// Restores the database from its snapshot.
		/// </summary>
		/// <param name="cnn">An active connection to the database.</param>
		/// <returns>true if the restore operation succeeded; false otherwise.</returns>
		private Boolean RestoreFromSnapshot(SqlConnection cnn)
		{
			var success = true;
			var snapshotName = String.Concat(this.databaseName, "_SS");

			using (var cmd = cnn.CreateCommand())
			{
				try
				{
					cmd.Connection = cnn;
					cmd.CommandTimeout = 1000;
					cmd.CommandText =
						String.Format("RESTORE DATABASE [{0}] FROM DATABASE_SNAPSHOT = '{0}_SS' WITH ENABLE_BROKER;", this.databaseName);
					cmd.ExecuteNonQuery();
				}
				catch (Exception ex)
				{
					success = false;
					Console.WriteLine($"An unexpected exception occurred while restoring database snapshots. Snapshot name: {snapshotName}");
					Console.WriteLine(ex);
				}
			}

			return success;
		}

		/// <summary>
		/// Immediately ends all service broker conversations.
		/// </summary>
		/// <param name="cnn">An active connection to the database.</param>
		private void SetErrorBrokerConversations(SqlConnection cnn)
		{
			using (var cmd = cnn.CreateCommand())
			{
				try
				{
					cmd.Connection = cnn;
					cmd.CommandTimeout = 120;
					cmd.CommandText = String.Format(
						"ALTER DATABASE [{0}] SET ERROR_BROKER_CONVERSATIONS WITH ROLLBACK IMMEDIATE;",
						this.databaseName);
					cmd.ExecuteNonQuery();
				}
				catch (Exception ex)
				{
					Console.WriteLine($"An unexpected exception occurred while attempting to end all service broker conversations. databaseName={this.databaseName}");
					Console.WriteLine(ex);
				}
			}
		}

		/// <summary>
		/// Puts the database in restricted mode.
		/// </summary>
		/// <param name="cnn">An active connection to the database.</param>
		private void SetRestrictedUserMode(SqlConnection cnn)
		{
			using (var setSingleConnectionCommand = cnn.CreateCommand())
			{
				try
				{
					setSingleConnectionCommand.Connection = cnn;
					setSingleConnectionCommand.CommandTimeout = 120;
					setSingleConnectionCommand.CommandText =
						String.Format("ALTER DATABASE [{0}] SET RESTRICTED_USER WITH ROLLBACK IMMEDIATE;", this.databaseName);
					setSingleConnectionCommand.ExecuteNonQuery();
				}
				catch (Exception ex)
				{
					Console.WriteLine($"An unexpected exception occurred while attempting to put the database in restricted mode. databaseName={this.databaseName}");
					Console.WriteLine(ex);
				}
			}
		}

		/// <summary>
		/// Puts the database in multi-user mode.
		/// </summary>
		/// <param name="cnn">An active connection to the database.</param>
		private void SetMultiUserMode(SqlConnection cnn)
		{
			using (var setMulitipleConnectionCommand = cnn.CreateCommand())
			{
				try
				{
					setMulitipleConnectionCommand.Connection = cnn;
					setMulitipleConnectionCommand.CommandTimeout = 120;
					setMulitipleConnectionCommand.CommandText =
						String.Format("ALTER DATABASE [{0}] SET MULTI_USER;", this.databaseName);
					setMulitipleConnectionCommand.ExecuteNonQuery();
				}
				catch (Exception ex)
				{
					Console.WriteLine("An unexpected exception occurred while attempting to put the database in multi-user mode.");
					Console.WriteLine(ex);
				}
			}
		}

		/// <summary>
		/// Determines if service broker is enabled for the database.
		/// </summary>
		/// <param name="cnn">An active connection to the database.</param>
		/// <returns>An object that can be cast to a <see cref="Boolean"/> unless it is null.</returns>
		private Object DetermineIfServiceBrokerEnabled(SqlConnection cnn)
		{
			using (var checkBroker = cnn.CreateCommand())
			{
				checkBroker.CommandText =
					String.Format("SELECT is_broker_enabled FROM sys.databases WHERE name = '{0}';", this.databaseName);
				checkBroker.Connection = cnn;
				checkBroker.CommandTimeout = 120;
				return checkBroker.ExecuteScalar();
			}
		}
	}
}
