using Microsoft.Data.SqlClient;

namespace IndexSynchronizerDevInit
{
	internal static class Runner
	{
		// TODO: all of this assumes both the source and target databases are on the same server,
		// which may be OK in the short term but will definitely need to rectify 
		internal static void Run()
		{
			var sourceServerName = @"localhost\sqlserver2019_d"; // TODO: from appsettings.json
			var sourceDatabaseName = "AdventureWorks2019"; // TOOD: from appsettings
			
			var newPassword =
				Convert.ToBase64String(
					System.Text.Encoding.UTF8.GetBytes(
						Guid.NewGuid().ToString()));

			try
			{
				
				var query = File.ReadAllText("Scripts/CreateTestRunnerLogin.sql");

				Console.WriteLine("Enter the password for source sa: ");
				var sourceSaPassword = GetPassword();

				// TODO: uncomment this when ready to pull target info
				//Console.WriteLine("Enter the password for target sa: ");
				//var targetSaPassword = GetPassword();

				query = query.Replace("#Password#", newPassword).Replace("#TargetDatabase#", sourceDatabaseName);

				// source
				CreateTestRunnerLogin(query, sourceDatabaseName, sourceServerName, sourceSaPassword);

				// TODO: uncomment this when ready to pull target info
				// target
				//CreateTestRunnerLogin(...);

			}
			catch (Exception ex)
			{
				Console.Write(ex);
			}
		}

		private static String GetPassword()
		{
			var readPassword = String.Empty;
			ConsoleKeyInfo keyInfo = new ConsoleKeyInfo();

			do
			{
				keyInfo = Console.ReadKey(intercept: true); // intercept true means to display characters in the console

				if (keyInfo.Key == ConsoleKey.Backspace && readPassword.Length > 0) // remove characters if there are characters to remove
				{
					readPassword = readPassword[0..^1]; // password is password - 1
					Console.Write("\b \b"); // move the cursor back and clear the last entered character
				}
				else if (!char.IsControl(keyInfo.KeyChar))
				{
					readPassword += keyInfo.KeyChar;
					Console.Write("*");
				}
			}
			while (keyInfo.Key != ConsoleKey.Enter);

			return readPassword;
		}

		private static void CreateTestRunnerLogin(
			String query, 
			String databaseName, 
			String serverName,
			String saPassword)
		{
			var connectionString =
				$"Server={serverName};" +
				$"Database={databaseName};" +
				$"User Id=sa;" +
				$"Password={saPassword};" +
				$"TrustServerCertificate=true";

			using (var connection = new SqlConnection(connectionString))
			using (var command = new SqlCommand(query, connection))
			{
				connection.Open();

				command.ExecuteNonQuery();

				connection.Close();
			}
		}

		private static void WriteTestRunnerCredentialsToRegistry(String testRunnerPassword, String testRunnerLogin = "IndexSyncTestLogin")
		{
			// TODO: this
		}
	}
}
