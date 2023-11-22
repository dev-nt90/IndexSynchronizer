using IndexSynchronizerServices.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IndexSynchronizerServices.Services
{
    public interface IConnectionStringBuilderService
    {
        public string BuildConnectionString(IConnectionDetails connectionDetails);
    }
}
