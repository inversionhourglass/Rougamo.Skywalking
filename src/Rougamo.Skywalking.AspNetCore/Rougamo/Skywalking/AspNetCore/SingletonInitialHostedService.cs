﻿using Microsoft.Extensions.Hosting;
using Rougamo.APM.Serialization;
using SkyApm.Config;
using SkyApm.Tracing;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace Rougamo.Skywalking.AspNetCore
{
    class SingletonInitialHostedService : IHostedService
    {
        private readonly IServiceProvider _provider;

        public SingletonInitialHostedService(IServiceProvider provider)
        {
            _provider = provider;
        }

        public Task StartAsync(CancellationToken cancellationToken)
        {
            SkywalkingSingleton.TracingContext = (ITracingContext)_provider.GetService(typeof(ITracingContext));
            SkywalkingSingleton.ConfigAccessor = (IConfigAccessor)_provider.GetService(typeof(IConfigAccessor));
            var serializer = _provider.GetService(typeof(ISerializer)) as ISerializer;
            if (serializer != null)
            {
                SkywalkingSingleton.Serializer = serializer;
            }

            return Task.CompletedTask;
        }

        public Task StopAsync(CancellationToken cancellationToken)
        {
            return Task.CompletedTask;
        }
    }
}
