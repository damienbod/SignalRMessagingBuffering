using Damienbod.SignalR.Host.Hubs;
using Damienbod.SignalR.IHubSync.Client;
using Damienbod.SignalR.IHubSync.Client.Dto;
using Damienbod.Slab;
using Damienbod.Slab.Services;
using Microsoft.AspNet.SignalR;

namespace Damienbod.SignalR.Host.Service
{
    public class SendHubSync : ISendHubSync
    {
        private readonly IHubLogger _slabLogger;
        private readonly IHubContext _hubContext;

        public SendHubSync(IHubLogger slabLogger)
        {
            _slabLogger = slabLogger;
            _hubContext = GlobalHost.ConnectionManager.GetHubContext<HubSync>(); 
        }

        public void SendSignalRMessageDto(SignalRMessageDto message)
        {
            _hubContext.Clients.All.SendSignalRMessageDto(message);
            _slabLogger.Log(HubType.HubServerVerbose, "MyHub Sending SendSignalRMessageDto");
        }

        public void RequestSpool()
        {
            _hubContext.Clients.All.RequestSpool();
            _slabLogger.Log(HubType.HubServerVerbose, "MyHub Sending RequestSpool");
        }
    }
}
