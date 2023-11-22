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

            sourceDatabaseSnapshot.Take();

            // TODO:
            //        targetDatabaseSnapshot = new DatabaseSnapshot(
            //config.TargetConnectionSettings.DatabaseName,
            //config.TargetConnectionSettings.MasterDatabaseConnectionString);
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
