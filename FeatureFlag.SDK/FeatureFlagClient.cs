using FeatureFlag.SDK.Models;
using FeatureFlag.SDK.Store;
using Microsoft.AspNetCore.SignalR.Client;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FeatureFlag.SDK
{
    public class FeatureFlagClient : IFeatureFlagClient, IAsyncDisposable
    {
        private readonly FeatureStore _store;
        private readonly EvaluatonEngine _engine;
        private readonly ILogger<FeatureFlagClient> _logger;
        private readonly HubConnection _hubConnection;

        public FeatureFlagClient(
            FeatureStore store,
            EvaluatonEngine engine,
            ILogger<FeatureFlagClient> logger,
            string adminApiUrl) {
        
            _store = store;
            _engine = engine;
            _logger = logger;

            //build SignalR connection
            _hubConnection = new HubConnectionBuilder()
                .WithUrl(adminApiUrl)
                .WithAutomaticReconnect(new[] { TimeSpan.Zero, TimeSpan.FromSeconds(2), TimeSpan.FromSeconds(10), TimeSpan.FromSeconds(30) })
                .Build();

            // Listen realtime updates from admin API
            _hubConnection.On<FeatureFlagDefinition>("ReceiveFlagUpdate", flag =>
            {
                _logger.LogInformation("Real-time update received for flag: {Key}", flag.Key);
                _store.UpdateFlag(flag);
            });

        }

        public async Task StartAsync(CancellationToken cancellationToken)
        {
            try
            {
                await _hubConnection.StartAsync(cancellationToken);
                _logger.LogInformation("Feature Flag SDK connected to Admin API successfully.");

                //To-Do
                //Initial HTTP GET here to populate the _stor with all current flags upon startup.
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Failed to connect Feature Flag SDK to Admin API. Operating on default fallbacks.");
            }
        }

        public bool IsFeatureEnabled(string featureKey, string userId)
        {
            var flag = _store.GetFlag(featureKey);

            //If the flag doesn't exist or isn't cached yet, default to false
            if (flag == null) return false;

            return _engine.Evaluate(userId, flag);
        }

        public async Task StopAsync(CancellationToken cancellationToken)
        {
            await _hubConnection.StopAsync(cancellationToken);
        }

        public async ValueTask DisposeAsync()
        {
            await _hubConnection.DisposeAsync();
        }

    }
}
