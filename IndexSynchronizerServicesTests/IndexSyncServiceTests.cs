using IndexSynchronizerServicesTests.TestInfrastructure;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IndexSynchronizerServicesTests
{
    [TestFixture]
    public sealed class IndexSyncServiceTests : SnapshotTestFixture
    {
        [Test]
        public void CanTakeDatabaseSnapshot()
        {
            Assert.Pass();
        }
    }
}
