using Microsoft.Data.SqlClient;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;



namespace IndexSynchronizerServicesTests.TestInfrastructure
{
    [TestFixture]
    public class SnapshotTestFixture
    {
        public DatabaseSnapshot sourceDatabaseSnapshot;
        public DatabaseSnapshot targetDatabaseSnapshot;

        public SettingProviderFactory settingProviderFactory;

        [OneTimeSetUp]
        public void OneTimeSetup()
        {
			settingProviderFactory = new SettingProviderFactory();

            sourceDatabaseSnapshot = new DatabaseSnapshot(
                "AdventureWorks", // TODO: from config
                settingProviderFactory.BuildMasterDbConnectionString(),
                settingProviderFactory.BuildDatabaseUnderTestConnectionString());

			using (var cnn = new SqlConnection(settingProviderFactory.BuildMasterDbConnectionString()))
			{
				using (IDbCommand cmd = cnn.CreateCommand())
				{
					cmd.Connection = cnn;
					cmd.CommandTimeout = 1000;
					cmd.CommandText = String.Format("select 1");
					cnn.Open();

					if ((Int32)cmd.ExecuteScalar() == 1)
					{
						throw new Exception("wat");
					}
				}
			}
			//sourceDatabaseSnapshot.Take();

			// TODO:
			//        targetDatabaseSnapshot = new DatabaseSnapshot(
			//config.TargetConnectionSettings.DatabaseName,
			//config.TargetConnectionSettings.MasterDatabaseConnectionString);
			// targetDatabaseSnapshot.Take();
		}

		//[SetUp]
  //      public virtual void SetUp()
  //      {
  //          // TODO: anything to do here? DI? config?
		//}

  //      [TearDown]
  //      public virtual void TearDown()
  //      {
		//	sourceDatabaseSnapshot.RestoreFromSnapshot();
		//	//targetDatabaseSnapshot.RestoreFromSnapshot();
  //      }

  //      [OneTimeTearDown]
  //      public void OneTimeTeardown()
  //      {
		//	sourceDatabaseSnapshot.RestoreFromSnapshot();
		//	sourceDatabaseSnapshot.DropDatabaseSnapshot();

		//	//targetDatabaseSnapshot.RestoreFromSnapshot();
		//	//targetDatabaseSnapshot.DropDatabaseSnapshot();
		//}
    }
}
