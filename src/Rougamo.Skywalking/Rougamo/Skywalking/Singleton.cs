using Rougamo.APM.Serialization;
using SkyApm.Config;
using SkyApm.Tracing;

namespace Rougamo.Skywalking
{
    public class Singleton
    {
        public static ITracingContext TracingContext;

        public static IConfigAccessor ConfigAccessor;

        public static ISerializer Serializer = new ToStringSerializer();
    }
}
