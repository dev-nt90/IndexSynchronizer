using IndexSynchronizerServices.Repositories;
using IndexSynchronizerServices.Services;

using Microsoft.Extensions.DependencyInjection;

namespace IndexSynchronizerServices.DependencyInjection
{
    public static class IndexSyncIServiceCollectionExtensions
    {
        public static IServiceCollection AddIndexSyncServices(this IServiceCollection services)
        {
            services.AddScoped<IIndexSyncService, IndexSynchronizerService>();
            services.AddScoped<IIndexDefinitionService, IndexDefinitionService>();

            return services;
        }

        public static IServiceCollection AddIndexSyncRepositories(this IServiceCollection services)
        {
            services.AddScoped<IIndexDefinitionRepository, IndexDefinitionRepository>();
            services.AddScoped<IIndexSyncRepository, IndexSyncRepository>();

            return services;
        }
    }
}
