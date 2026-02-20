using Microsoft.AspNetCore.SignalR;

namespace FeatureFlag.AdminAPI.Hubs
{
    //Inherit Hub for web socket communication
    public class FlagHub: Hub
    {
        // We don't need any methods here for the Client to call *in*
        // We only use this Hub to send messages *out* from the Controller.
    }
}
