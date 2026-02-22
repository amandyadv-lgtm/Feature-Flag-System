using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FeatureFlag.SDK.Models
{
    public class FeatureFlagDefinition
    {
        public string Key { get; set; } = string.Empty;
        public bool IsEnabled { get; set; }

        // For Percentage Rollouts (0 to 100)
        public int RolloutPercentage { get; set; }

        // For specific user targetting (e.g. beta testers)
        public HashSet<string> TargetUserIds { get; set; } = new();
    }
}
