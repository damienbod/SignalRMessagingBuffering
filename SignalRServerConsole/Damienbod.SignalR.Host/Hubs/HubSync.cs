using System;
using System.Threading.Tasks;
using Damienbod.SignalR.IHubSync.Client.Dto;
using Damienbod.Slab;
using Damienbod.Slab.Services;
using Microsoft.AspNet.SignalR;

namespace Damienbod.SignalR.Host.Hubs
{
    public class HubSync : Hub
    {
        private readonly IHubLogger _slabLogger;

        public HubSync(IHubLogger slabLogger)
        {
            _slabLogger = slabLogger;
        }


        public void SendSignalRMessageDto(SignalRMessageDto message)
        {
            Console.WriteLine("Server Recieved SignalRMessageDto " + message.String1 + ", " + message.String2);
            _slabLogger.Log(HubType.HubServerVerbose, "HubSync Sending SendHelloObject " + message.String1 + " " + message.String2);
            Clients.All.sendHelloObject(message);
        }

        public override Task OnConnected()
        {
            _slabLogger.Log(HubType.HubServerVerbose, "HubSync OnConnected" + Context.ConnectionId);
            return (base.OnConnected());
        }

        public override Task OnDisconnected()
        {
            _slabLogger.Log(HubType.HubServerVerbose, "HubSync OnDisconnected" + Context.ConnectionId);
            return (base.OnDisconnected());
        }

        public override Task OnReconnected()
        {
            _slabLogger.Log(HubType.HubServerVerbose, "HubSync OnReconnected" + Context.ConnectionId);
            return (base.OnReconnected());
        }
    }
}