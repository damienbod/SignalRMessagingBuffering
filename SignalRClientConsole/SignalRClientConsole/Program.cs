using System;
using System.Timers;
using Damienbod.SignalR.IHubSync.Client.Dto;
using Microsoft.AspNet.SignalR.Client;
using SignalRClientConsole.HubClients;
using SignalRClientConsole.Logging;
using SignalRClientConsole.Spool;

namespace SignalRClientConsole
{
    public class Program
    {
        private static Timer _aTimer;
        private static void OnTimedEvent(object source, ElapsedEventArgs e)
        {
            if (MyHubClient.SendSpool)
            {
                Console.WriteLine("Items to be spooled: {0}", MyHubClient.SpoolCount);
                if (myHubClient.State == ConnectionState.Connected)
                {
                    while (spoolDataRepository.GetNextSignalRMessageDto() != null)
                    {
                        var spoolMessage = spoolDataRepository.GetNextSignalRMessageDto();
                        myHubClient.SendSignalRMessageDto(spoolMessage);
                        spoolDataRepository.RemoveSignalRMessageDto(spoolMessage.Id);
                        
                    }

                    MyHubClient.SpoolCount = 0;
                }

                MyHubClient.SendSpool = false;
            }
        }

        static readonly MyHubClient myHubClient = new MyHubClient();
        static readonly SpoolDataRepository spoolDataRepository = new SpoolDataRepository();

        static void Main(string[] args)
        {
            Console.WriteLine("Starting client  http://localhost:8089");
            Console.WriteLine("----------------------");
            Console.WriteLine("H - Help");
            Console.WriteLine("S - Send message or add message to spool");
            Console.WriteLine("C - Close hub connection");
            Console.WriteLine("N - Start new hub connection");
            Console.WriteLine("X - close application");
            Console.WriteLine("----------------------");
            
            _aTimer = new Timer(10000);
            _aTimer.Elapsed += new ElapsedEventHandler(OnTimedEvent);

            // Set the Interval to 2 seconds (2000 milliseconds).
            _aTimer.Interval = 2000;
            _aTimer.Enabled = true;

            while (true)
            {
                string key = Console.ReadLine();

                if (key.ToUpper() == "S")
                {
                    if (myHubClient.State == ConnectionState.Connected && MyHubClient.SpoolCount <= 0)
                    {
                        var message = new SignalRMessageDto {String1 = "clientMessage String1", String2 = " ,String2"};
                        myHubClient.SendSignalRMessageDto(message);
                    }
                    else
                    {
                        Console.WriteLine(" :no connection or spool not empty, adding message to spool");
                        spoolDataRepository.AddSignalRMessageDto(new SignalRMessageDto()
                        {
                            String1 = "client message: String1",
                            String2 = " ,String2",
                            Int1 = 3,
                            Int2 = 3
                        });
                        HubClientEvents.Log.Warning("Can't send message, connectionState= " + myHubClient.State);
                        MyHubClient.SpoolCount++;
                    }

                }
                if (key.ToUpper() == "C")
                {
                    myHubClient.CloseHub();
                    Console.WriteLine(" :closing hub if opened");
                    HubClientEvents.Log.Informational("Closed Hub");
                }
                if (key.ToUpper() == "N")
                {
                    myHubClient.StartHub();
                    Console.WriteLine(" :starting a new  hub if server exists");
                    HubClientEvents.Log.Informational("Started the Hub");
                }
                if (key.ToUpper() == "X")
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
