using Rougamo.APM.Serialization;
using SkyApm.Config;
using SkyApm.Tracing;

namespace Rougamo.Skywalking
{
    /// <summary>
    /// </summary>
    public class SkywalkingSingleton
    {
        /// <summary>
        /// <see cref="ITracingContext"/>
        /// </summary>
        public static ITracingContext TracingContext;

        /// <summary>
        /// <see cref="IConfigAccessor"/>
        /// </summary>
        public static IConfigAccessor ConfigAccessor;

        /// <summary>
        /// parameter and return value serializer, default <see cref="ToStringSerializer"/>
        /// </summary>
        public static ISerializer Serializer = new ToStringSerializer();
    }
}
