using System;
using System.Net.Mail;
using Damienbod.SignalR.IHubSync.Client.Dto;
using Microsoft.AspNet.SignalR.Client;
using SignalRClientConsole.HubClients;
using SignalRClientConsole.Logging;
using SignalRClientConsole.Spool;

namespace SignalRClientConsole
{
    class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine("Starting client  http://localhost:8089");      
            var myHubClient = new MyHubClient();
            var spoolDataRepository = new SpoolDataRepository();
            
            while (true)
            {
                string key = Console.ReadLine();

                if (key.ToUpper() == "I")
                {
                    spoolDataRepository.AddSignalRMessageDto(new SignalRMessageDto() { String1 = "tring1", String2 = "tring2", Int1 = 2, Int2 = 3 });
                }
                if (key.ToUpper() == "D")
                {
                    spoolDataRepository.RemoveSignalRMessageDto(1);
                }
                if (key.ToUpper() == "G")
                {
                    spoolDataRepository.GetNextSignalRMessageDto();
                }

                if (key.ToUpper() == "S")
                {
                    if (myHubClient.State == ConnectionState.Connected)
                    {
                        // Send spool messages first                       
                        while(spoolDataRepository.GetNextSignalRMessageDto()  != null)
                        {
                            var spoolMessage = spoolDataRepository.GetNextSignalRMessageDto();
                            myHubClient.SendSignalRMessageDto(spoolMessage);
                            spoolDataRepository.RemoveSignalRMessageDto(spoolMessage.Id);
                        }

                        var message = new SignalRMessageDto { String1 = "clientMessage String1", String2 = "clientMessage String2" };
                        myHubClient.SendSignalRMessageDto(message);
                    }
                    else
                    {
                        spoolDataRepository.AddSignalRMessageDto(new SignalRMessageDto() { String1 = "clientMessage String1", String2 = "clientMessage String2", Int1 = 3, Int2 = 3 });
                        HubClientEvents.Log.Warning("Can't send message, connectionState= " + myHubClient.State);
                    }
                    
                }
                if (key.ToUpper() == "T")
                {
                    myHubClient.CloseHub();
                    HubClientEvents.Log.Informational("Closed Hub");
                }
                if (key.ToUpper() == "Z")
                {
                    myHubClient.StartHub();
                    HubClientEvents.Log.Informational("Started the Hub");
                }
                if (key.ToUpper() == "C")
                {
                    break;
                }
            }

        }
    }
}
