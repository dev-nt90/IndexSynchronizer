using IndexSynchronizerServices.DependencyInjection;
using Microsoft.Extensions.DependencyInjection;

namespace IndexSynchronizerServicesTests.TestInfrastructure.DependencyInjection
{
	internal static class SoftwareUnderTestInjector
	{
		public static IServiceCollection InjectSoftwareUnderTest(this IServiceCollection services)
		{
			return services
				.AddIndexSyncRepositories()
				.AddIndexSyncServices();
		}
	}
}
