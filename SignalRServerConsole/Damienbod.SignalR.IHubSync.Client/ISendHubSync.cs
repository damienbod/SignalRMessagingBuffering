using Damienbod.SignalR.IHubSync.Client.Dto;

namespace Damienbod.SignalR.IHubSync.Client
{
    public interface ISendHubSync
    {
        void SendSignalRMessageDto(SignalRMessageDto message);

        void RequestSpool();

    }
}
