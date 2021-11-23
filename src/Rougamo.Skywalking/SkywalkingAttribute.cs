using Rougamo.Context;
using SkyApm.Config;
using SkyApm.Tracing;
using SkyApm.Tracing.Segments;
using System.Text;

namespace Rougamo.Skywalking
{
    /// <summary>
    /// skywalking local span annotation
    /// </summary>
    public class SkywalkingAttribute : MoAttribute
    {
        private SegmentContext _segmentContext;

        /// <summary>
        /// donot record return value, false by default
        /// </summary>
        public bool IgnoreReturn { get; set; }

        /// <summary>
        /// <inheritdoc/>
        /// </summary>
        public override void OnEntry(MethodContext context)
        {
            if (Singleton.TracingContext == null) return;

            _segmentContext = Singleton.TracingContext.CreateLocalSegmentContext($"{context.TargetType.FullName}.{context.Method.Name}");
            _segmentContext.Span.AddLog(LogEvent.Message("parameters: " + GetMethodParameters(context)));
        }

        /// <summary>
        /// <inheritdoc/>
        /// </summary>
        public override void OnSuccess(MethodContext context)
        {
            if (_segmentContext == null || IgnoreReturn) return;

            _segmentContext.Span.AddLog(LogEvent.Message("return: " + Singleton.Serializer.Serialize(context.ReturnValue)));
        }

        /// <summary>
        /// <inheritdoc/>
        /// </summary>
        public override void OnException(MethodContext context)
        {
            if (_segmentContext == null || Singleton.ConfigAccessor == null) return;

            _segmentContext.Span.ErrorOccurred(context.Exception, Singleton.ConfigAccessor.Get<TracingConfig>());
        }

        /// <summary>
        /// <inheritdoc/>
        /// </summary>
        public override void OnExit(MethodContext context)
        {
            if (_segmentContext == null || Singleton.TracingContext == null) return;

            Singleton.TracingContext.Release(_segmentContext);
        }

        private string GetMethodParameters(MethodContext context)
        {
            var ignores = context.Method.GetIgnoreArgs();
            var count = context.Arguments.Length;
            var builder = new StringBuilder();
            for (int i = 0; i < count; i++)
            {
                var value = (ignores & (1 << i)) != 0 ? "***" : context.Arguments[i];
                builder.Append($"arg{i}={Singleton.Serializer.Serialize(value)}&");
            }
            return builder.ToString();
        }
    }
}
