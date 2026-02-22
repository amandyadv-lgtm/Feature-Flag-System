using FeatureFlag.SDK.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FeatureFlag.SDK
{
    public class EvaluatonEngine
    {
        public bool Evaluate(string userid, FeatureFlagDefinition flag)
        {
            if (!flag.IsEnabled)
            {
                return false;
            }

            if (flag.TargetUserIds.Contains(userid))
            {
                return true;
            }

            if(flag.RolloutPercentage > 0)
            {
                var hash = GetDeterministicHash($"{userid} - {flag.Key}");
                var userbucket = hash % 100;
                return userbucket < flag.RolloutPercentage;
            }
            return false;
        }

        private uint GetDeterministicHash(string input)
        {
            // FNV-1a Hashing Algorithm: Fast and evenly distributed
            uint hash = 2166136261;
            foreach (byte b in Encoding.UTF8.GetBytes(input))
            {
                hash = (hash ^ b) * 16777619;
            }
            return hash;
        }
    }
}
