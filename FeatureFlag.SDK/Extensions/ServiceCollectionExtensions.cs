using FeatureFlag.SDK.Store;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FeatureFlag.SDK.Extensions
{
    public static class ServiceCollectionExtensions
    {
        public static IServiceCollection AddFeatureFlagSystem(this IServiceCollection services, string adminApiUrl)
        {
            //Register the pure logic classes as Singletons

            services.AddSingleton<FeatureStore>();
            services.AddSingleton<EvaluatonEngine>();

            //Register the Client implementation
            services.AddSingleton<IFeatureFlagClient>(sp =>
            {
                var store = sp.GetRequiredService<FeatureStore>();
                var engine = sp.GetRequiredService<EvaluatonEngine>();
                var logger = sp.GetRequiredService<ILogger<FeatureFlagClient>>();

                return new FeatureFlagClient(store, engine, logger, adminApiUrl);
            });

            return services;
        }
    }
}
