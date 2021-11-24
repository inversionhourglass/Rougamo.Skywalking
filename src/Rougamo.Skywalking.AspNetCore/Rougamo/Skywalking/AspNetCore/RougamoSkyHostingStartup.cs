using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.DependencyInjection;
using Rougamo.Skywalking.AspNetCore;

[assembly: HostingStartup(typeof(RougamoSkyHostingStartup))]

namespace Rougamo.Skywalking.AspNetCore
{
    internal class RougamoSkyHostingStartup : IHostingStartup
    {
        public void Configure(IWebHostBuilder builder)
        {
            builder.ConfigureServices(services => services.AddRougamoSkywalking());
        }
    }
}
