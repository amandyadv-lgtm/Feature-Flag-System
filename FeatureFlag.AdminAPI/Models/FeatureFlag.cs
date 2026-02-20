namespace FeatureFlag.AdminAPI.Models
{
    public class FeatureFlag
    {
        public int Id { get; set; }
        public string Key { get; set; } = string.Empty; // e.g., "new-checkout-flow"
        public bool IsEnabled { get; set; }
        public int RolloutPercentage { get; set; }

        public string TargetedUsers { get; set; } = string.Empty;
    }
}
