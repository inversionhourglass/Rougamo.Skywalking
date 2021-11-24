using Rougamo.APM;
using Rougamo.Context;
using SkyApm.Config;
using SkyApm.Tracing;
using SkyApm.Tracing.Segments;

namespace Rougamo.Skywalking
{
    /// <summary>
    /// skywalking local span annotation
    /// </summary>
    public class SkywalkingAttribute : MoAttribute
    {
        private SegmentContext _segmentContext;

        /// <summary>
        /// use method full name if not set this property
        /// </summary>
        public string OperationName { get; set; }

        /// <summary>
        /// donot record return value, false by default
        /// </summary>
        public bool IgnoreReturn { get; set; }

        /// <summary>
        /// when exception occurred, record the exception and mark the exception as recorded, 
        /// outer <see cref="SkywalkingAttribute"/> will not record it again, true by default
        /// </summary>
        public bool MarkException { get; set; } = true;

        /// <summary>
        /// <inheritdoc/>
        /// </summary>
        public override void OnEntry(MethodContext context)
        {
            if (Singleton.TracingContext == null) return;

            var operationName = string.IsNullOrEmpty(OperationName) ? $"{context.TargetType.FullName}.{context.Method.Name}" : OperationName;
            _segmentContext = Singleton.TracingContext.CreateLocalSegmentContext(operationName);
            _segmentContext.Span.AddLog(LogEvent.Message("parameters: " + context.GetMethodParameters(Singleton.Serializer)));
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

            if(MarkException)
            {
                if (context.Exception.Data.Contains(Constants.EXCEPTION_MARK))
                {
                    _segmentContext.Span.IsError = true;
                }
                else
                {
                    _segmentContext.Span.ErrorOccurred(context.Exception, Singleton.ConfigAccessor.Get<TracingConfig>());
                    context.Exception.Data.Add(Constants.EXCEPTION_MARK, null);
                }
            }
            else
            {
                _segmentContext.Span.ErrorOccurred(context.Exception, Singleton.ConfigAccessor.Get<TracingConfig>());
            }
        }

        /// <summary>
        /// <inheritdoc/>
        /// </summary>
        public override void OnExit(MethodContext context)
        {
            if (_segmentContext == null || Singleton.TracingContext == null) return;

            Singleton.TracingContext.Release(_segmentContext);
        }
    }
}
