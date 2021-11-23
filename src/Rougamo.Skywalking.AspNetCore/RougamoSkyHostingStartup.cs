using Microsoft.AspNetCore.Hosting;
using Rougamo.Skywalking.AspNetCore;
using SkyApm.Config;
using SkyApm.Tracing;

[assembly: HostingStartup(typeof(RougamoSkyHostingStartup))]

namespace Rougamo.Skywalking.AspNetCore
{
    internal class RougamoSkyHostingStartup : IHostingStartup
    {
        public void Configure(IWebHostBuilder builder)
        {
            builder.Configure(applicationBuilder =>
            {
                Singleton.TracingContext = (ITracingContext)applicationBuilder.ApplicationServices.GetService(typeof(ITracingContext));
                Singleton.ConfigAccessor = (IConfigAccessor)applicationBuilder.ApplicationServices.GetService(typeof(IConfigAccessor));
            });
        }
    }
}
