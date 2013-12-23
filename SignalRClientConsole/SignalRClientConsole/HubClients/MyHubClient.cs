using System;
using Damienbod.SignalR.IHubSync.Client;
using Damienbod.SignalR.IHubSync.Client.Dto;
using Microsoft.AspNet.SignalR.Client;
using SignalRClientConsole.Logging;

namespace SignalRClientConsole.HubClients
{
    public class MyHubClient : BaseHubClient, ISendHubSync, IRecieveHubSync
    {
        public MyHubClient()
        {
            Init();
        }

        public new void Init()
        {
            HubConnectionUrl = "http://localhost:8089/";
            HubProxyName = "Hubsync";
            HubTraceLevel = TraceLevels.None;
            HubTraceWriter = Console.Out;

            base.Init();

            _myHubProxy.On<SignalRMessageDto>("SendSignalRMessageDto", Recieve_SendSignalRMessageDto);

            StartHubInternal();
        }

        public override void StartHub()
        {
            _hubConnection.Dispose();
            Init();
        }

        public void Recieve_SendSignalRMessageDto(SignalRMessageDto message)
        {
            Console.WriteLine("Recieved SignalRMessageDto " + message.String1 + ", " + message.String2);
            HubClientEvents.Log.Informational("Recieved sendHelloObject " + message.String1 + ", " + message.String2);
        }

        public void SendSignalRMessageDto(SignalRMessageDto message)
        {
            _myHubProxy.Invoke<SignalRMessageDto>("SendSignalRMessageDto", message).ContinueWith(task =>
            {
                if (task.IsFaulted)
                {
                    HubClientEvents.Log.Error("There was an error opening the connection:" + task.Exception.GetBaseException());
                }

            }).Wait();
            HubClientEvents.Log.Informational("Client sendHelloObject sent to server");
        }
    }
}
