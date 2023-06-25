using DCR.WebApi.Repositories;

namespace DCR.WebApi.Scope.Extensions
{
    public static class InjectionServiceCollectionExtensions
    {
        public static void AddCustomServices(this IServiceCollection services)
        {
            services.AddSingleton<IFruitRepository, FruitRepository>();
        }
    }
}
