using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FeatureFlag.SDK
{
    public interface IFeatureFlagClient
    {

        bool IsFeatureEnabled(string featureKey, string userId);

        Task StartAsync(CancellationToken cancellationToken =  default);

        Task StopAsync(CancellationToken cancellationToken = default);
    }
}
