using FeatureFlag.SDK;
using FeatureFlag.SDK.Extensions;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;

var host = Host.CreateDefaultBuilder(args)
    .ConfigureServices(services =>
    {
        services.AddFeatureFlagSystem("http://localhost:5056/flaghub");
    }).
    Build();

var flagClient = host.Services.GetRequiredService<IFeatureFlagClient>();
var logger = host.Services.GetRequiredService<ILogger<Program>>();

Console.WriteLine("--- Starting Feature Flag Consumer ---");

//Connect SignalR hub

await flagClient.StartAsync();

// 4. The Simulation Loop
// We will check the flag "new-checkout" for user "user-123" every second.

var featureKey = "new-checkout";
var userId = "user-123";

while (true)
{
    bool isEnabled = flagClient.IsFeatureEnabled(featureKey, userId);

    if (isEnabled)
    {
        Console.ForegroundColor = ConsoleColor.Green;
        Console.WriteLine($"[{DateTime.Now:HH:mm:ss}] Feature '{featureKey}' is ON for {userId}");
    }
    else
    {
        Console.ForegroundColor = ConsoleColor.Red;
        Console.WriteLine($"[{DateTime.Now:HH:mm:ss}] Feature '{featureKey}' is OFF for {userId}");
    }

    Console.ResetColor();
    await Task.Delay(2000);
}