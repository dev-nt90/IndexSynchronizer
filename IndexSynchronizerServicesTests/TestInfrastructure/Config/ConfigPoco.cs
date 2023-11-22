using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IndexSynchronizerServicesTests.TestInfrastructure.Config
{
    public class ConfigPoco
    {
        public SqlServerConnectionSettings SourceConnectionSettings { get; set; }
        public SqlServerConnectionSettings TargetConnectionSettings { get; set; }
    }
}
