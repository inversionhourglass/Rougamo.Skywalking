using System.Collections.Concurrent;
using System.Reflection;

namespace Rougamo.Skywalking
{
    internal static class RefectionExtensions
    {
        private static ConcurrentDictionary<MethodBase, long> _Ignores = new ConcurrentDictionary<MethodBase, long>();

        public static long GetIgnoreArgs(this MethodBase method)
        {
            return _Ignores.GetOrAdd(method, GetIgnores);
        }

        private static long GetIgnores(MethodBase method)
        {
            var ignores = 0L;
            var parameters = method.GetParameters();
            var methodAttrs = method.GetCustomAttributes(typeof(SkyIgnoreAttribute), true);
            var length = parameters.Length > 64 ? 64 : parameters.Length;
            if (methodAttrs.Length != 0)
            {
                for (var i = 0; i < length; i++)
                {
                    ignores |= 1L << i;
                }
            }
            else
            {
                for (var i = 0; i < length; i++)
                {
                    var paraAttrs = parameters[i].GetCustomAttributes(typeof(SkyIgnoreAttribute), true);
                    if (paraAttrs.Length != 0)
                    {
                        ignores |= 1L << i;
                    }
                }
            }

            return ignores;
        }
    }
}
