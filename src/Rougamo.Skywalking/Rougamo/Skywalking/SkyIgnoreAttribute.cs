using Rougamo.APM;
using System;

namespace Rougamo.Skywalking
{
    /// <summary>
    /// skywalking ignore parameter value record
    /// </summary>
    [AttributeUsage(AttributeTargets.Method | AttributeTargets.Parameter)]
    public class SkyIgnoreAttribute : ApmIgnoreAttribute
    {
    }
}
