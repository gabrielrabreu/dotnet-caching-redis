namespace DCR.WebApi.Scope.Extensions
{
    public static class CacheServiceCollectionExtensions
    {
        public static void AddCustomCache(this IServiceCollection services, IConfiguration configuration)
        {
            services.AddStackExchangeRedisCache(options =>
            {
                options.Configuration = configuration["ConnectionStrings:Redis"];
            });
        }
    }
}
