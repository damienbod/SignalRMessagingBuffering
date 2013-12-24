using System;
using Damienbod.SignalR.IHubSync.Client.Dto;
using Microsoft.AspNet.SignalR.Client;
using SignalRClientConsole.HubClients;
using SignalRClientConsole.Logging;
using SignalRClientConsole.Spool;

namespace SignalRClientConsole
{
    public class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine("Starting client  http://localhost:8089");
            Console.WriteLine("----------------------");
            Console.WriteLine("H - Help");
            Console.WriteLine("S - Send message or add message to spool");
            Console.WriteLine("T - Close hub connection");
            Console.WriteLine("Z - Start new hub connection");
            Console.WriteLine("C - close application");
            Console.WriteLine("----------------------");
            var myHubClient = new MyHubClient();
            var spoolDataRepository = new SpoolDataRepository();
            
            while (true)
            {
                if (MyHubClient.SendSpool)
                {
                    if (myHubClient.State == ConnectionState.Connected)
                    {
                        while (spoolDataRepository.GetNextSignalRMessageDto() != null)
                        {
                            var spoolMessage = spoolDataRepository.GetNextSignalRMessageDto();
                            myHubClient.SendSignalRMessageDto(spoolMessage);
                            spoolDataRepository.RemoveSignalRMessageDto(spoolMessage.Id);
                        }
                    }
                    MyHubClient.SendSpool = false;
                }
                else
                {
                    string key = Console.ReadLine();

                    if (key.ToUpper() == "S")
                    {
                        if (myHubClient.State == ConnectionState.Connected)
                        {
                            var message = new SignalRMessageDto { String1 = "clientMessage String1", String2 = "clientMessage String2" };
                            myHubClient.SendSignalRMessageDto(message);
                        }
                        else
                        {
                            Console.WriteLine("no connection, adding message to spool");
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
                    if (key.ToUpper() == "H")
                    {
                        Console.WriteLine("----------------------");
                        Console.WriteLine("H - Help");
                        Console.WriteLine("S - Send message or add message to spool");
                        Console.WriteLine("T - Close hub connection");
                        Console.WriteLine("Z - Start new hub connection");
                        Console.WriteLine("C - close application");
                        Console.WriteLine("----------------------");
                    }
                
                }
                
            }

        }
    }
}
