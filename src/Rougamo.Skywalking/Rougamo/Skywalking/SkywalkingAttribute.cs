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
        public virtual string OperationName { get; set; }

        /// <summary>
        /// record parameter and return value and working with <see cref="ApmIgnoreAttribute"/> if true, otherwise dot not record those and working with <see cref="ApmRecordAttribute"/>, default true
        /// </summary>
        public virtual bool RecordArguments { get; set; } = true;

        /// <summary>
        /// <inheritdoc/>
        /// </summary>
        public override void OnEntry(MethodContext context)
        {
            if (SkywalkingSingleton.TracingContext == null) return;

            var operationName = string.IsNullOrEmpty(OperationName) ? $"{context.TargetType.FullName}.{context.Method.Name}" : OperationName;
            var parameterString = context.GetMethodParameters(SkywalkingSingleton.Serializer, RecordArguments);
            _segmentContext = SkywalkingSingleton.TracingContext.CreateLocalSegmentContext(operationName);
            _segmentContext.Span.AddLog(LogEvent.Message("parameters: " + parameterString));
        }

        /// <summary>
        /// <inheritdoc/>
        /// </summary>
        public override void OnSuccess(MethodContext context)
        {
            if (_segmentContext == null) return;

            var returnString = context.GetMethodReturnValue(SkywalkingSingleton.Serializer, RecordArguments);
            _segmentContext.Span.AddLog(LogEvent.Message("return: " + returnString));
        }

        /// <summary>
        /// <inheritdoc/>
        /// </summary>
        public override void OnException(MethodContext context)
        {
            if (_segmentContext == null || SkywalkingSingleton.ConfigAccessor == null) return;

            if(context.IsMuteExceptionForApm(RecordArguments))
            {
                if (context.Exception.Data.Contains(SkywalkingConstants.EXCEPTION_MARK))
                {
                    _segmentContext.Span.IsError = true;
                }
                else
                {
                    _segmentContext.Span.ErrorOccurred(context.Exception, SkywalkingSingleton.ConfigAccessor.Get<TracingConfig>());
                    context.Exception.Data.Add(SkywalkingConstants.EXCEPTION_MARK, null);
                }
            }
            else
            {
                _segmentContext.Span.ErrorOccurred(context.Exception, SkywalkingSingleton.ConfigAccessor.Get<TracingConfig>());
            }
        }

        /// <summary>
        /// <inheritdoc/>
        /// </summary>
        public override void OnExit(MethodContext context)
        {
            if (_segmentContext == null || SkywalkingSingleton.TracingContext == null) return;

            SkywalkingSingleton.TracingContext.Release(_segmentContext);
        }
    }
}
