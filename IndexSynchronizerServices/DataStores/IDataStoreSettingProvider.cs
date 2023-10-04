using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IndexSynchronizerServices.DataStores
{
    public interface IDataStoreSettingProvider
    {
        SqlServerSettings SqlServerSettings { get; }

        // TODO: postgres, etc
    }
}
