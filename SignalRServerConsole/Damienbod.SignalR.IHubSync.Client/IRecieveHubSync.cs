using Damienbod.SignalR.IHubSync.Client.Dto;

namespace Damienbod.SignalR.IHubSync.Client
{
    public interface IRecieveHubSync
    {
        void Recieve_SendSignalRMessageDto(SignalRMessageDto message);

        void Recieve_RequestSpool();
    }
}
