using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
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
            var config = settingProviderFactory.CreateConfig();


            sourceDatabaseSnapshot = new DatabaseSnapshot(
                config.SourceConnectionSettings.DatabaseName, 
                config.SourceConnectionSettings.MasterDatabaseConnectionString);
            sourceDatabaseSnapshot.Take();

            targetDatabaseSnapshot = new DatabaseSnapshot(
				config.TargetConnectionSettings.DatabaseName,
				config.TargetConnectionSettings.MasterDatabaseConnectionString);
			// targetDatabaseSnapshot.Take();
        }

		[SetUp]
        public virtual void SetUp()
        {
            // TODO: anything to do here? DI? config?
		}

        [TearDown]
        public virtual void TearDown()
        {
			sourceDatabaseSnapshot.RestoreFromSnapshot();
			//targetDatabaseSnapshot.RestoreFromSnapshot();
        }

        [OneTimeTearDown]
        public void OneTimeTeardown()
        {
			sourceDatabaseSnapshot.RestoreFromSnapshot();
			sourceDatabaseSnapshot.DropDatabaseSnapshot();

			//targetDatabaseSnapshot.RestoreFromSnapshot();
			//targetDatabaseSnapshot.DropDatabaseSnapshot();
		}
    }
}
