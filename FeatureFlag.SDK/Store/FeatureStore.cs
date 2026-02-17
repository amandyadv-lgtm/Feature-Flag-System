using FeatureFlag.SDK.Models;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FeatureFlag.SDK.Store
{
    public class FeatureStore
    {
        private readonly ConcurrentDictionary<string, FeatureFlagDefinition> _flags = new();

        public void UpdateFlag(FeatureFlagDefinition flag)
        {
            _flags.AddOrUpdate(flag.Key, flag, (key, oldvalue) => flag);

        }

        public FeatureFlagDefinition? GetFlag(string key) 
        {
            _flags.TryGetValue(key, out var flag);
             return flag; 
        }
    }
}
