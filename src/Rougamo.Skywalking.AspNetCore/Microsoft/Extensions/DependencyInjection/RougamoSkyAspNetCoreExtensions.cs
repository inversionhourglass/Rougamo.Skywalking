using Rougamo.Skywalking.AspNetCore;

namespace Microsoft.Extensions.DependencyInjection
{
    /// <summary>
    /// </summary>
    public static class RougamoSkyAspNetCoreExtensions
    {
        /// <summary>
        /// add rougamo annotation of skywalking
        /// </summary>
        public static IServiceCollection AddRougamoSkywalking(this IServiceCollection services)
        {
            return services.AddHostedService<SingletonInitialHostedService>();
        }
    }
}
