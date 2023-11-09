using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Transactions;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using IndexSynchronizerServicesTests.TestInfrastructure.DependencyInjection;

namespace IndexSynchronizerServicesTests.TestInfrastructure
{
    // TODO: lots
    public abstract class TransactionalTestFixture
    {
        private TransactionScope transactionScope;
        private IServiceScope serviceScope;

        protected IServiceProvider serviceProvider { get; private set; }

        [SetUp]
        public virtual void SetUp()
        {
            serviceScope = ConfigureServices()
                .BuildServiceProvider()
                .CreateScope();

            serviceProvider = serviceScope.ServiceProvider;

            transactionScope =
                new TransactionScope(

                    // the start of a new transactions generate a new ambient transaction always
                    TransactionScopeOption.RequiresNew,

                    // a transactional test is not expected to commit its work, so any reads will need to be dirty
                    new TransactionOptions { IsolationLevel = IsolationLevel.ReadUncommitted },

                    // allows transactions to play nice with async/await
                    TransactionScopeAsyncFlowOption.Enabled);
        }

        [TearDown]
        public virtual void TearDown()
        {
            // callers must call this method to dispose of ambient transactions.. is it possible to enforce or design around?
            transactionScope?.Dispose();
            serviceScope?.Dispose();
        }

        protected virtual IServiceCollection ConfigureServices()
        {
            // TODO: add relevant settings (file stores, data stores, credentials, all that jazz)
            var settingProviderFactory = new SettingProviderFactory();
            
            var services =
                new ServiceCollection()
                    .AddLogging()
                    //.AddScoped(_ => settingProviderFactory.CreateConfig())
                    .InjectSoftwareUnderTest()
					;

            return services;
        }

        
    }
}
