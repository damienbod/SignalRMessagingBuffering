﻿using System;
using System.Diagnostics;
using Damienbod.SignalR.Host.Service;
using Damienbod.SignalR.Host.Unity;
using Damienbod.SignalR.IHubSync.Client;
using Damienbod.SignalR.IHubSync.Client.Dto;
using Microsoft.AspNet.SignalR;
using Microsoft.AspNet.SignalR.Hubs;
using Microsoft.Owin.Cors;
using Microsoft.Owin.Hosting;
using Microsoft.Owin;
using Microsoft.Practices.Unity;
using Owin;

[assembly: OwinStartup(typeof(Damienbod.SignalR.Host.Startup))]
namespace Damienbod.SignalR.Host
{
    public class Startup
    {
        private static ISendHubSync _myHub;

        public static void Start()
        {
            GlobalHost.DependencyResolver.Register(typeof(IHubActivator), () => new UnityHubActivator(UnityConfiguration.GetConfiguredContainer())); 
            GlobalHost.HubPipeline.AddModule(new LoggingPipelineModule());
            GlobalHost.HubPipeline.AddModule(new ErrorHandlingPipelineModule());
            
            var url = MyConfiguration.GetInstance().MyHubServiceUrl();
            _myHub = UnityConfiguration.GetConfiguredContainer().Resolve<ISendHubSync>();
            
            using (WebApp.Start(url))
            {
                Console.WriteLine("Server running on {0}", url);
                Console.WriteLine("----------------------");
                Console.WriteLine("H - Help");
                Console.WriteLine("S - Send message or add message to spool");
                Console.WriteLine("R - request spool");
                Console.WriteLine("C - close application");
                Console.WriteLine("----------------------");
                while (true)
                {
                    var key = Console.ReadLine();
                    if (key.ToUpper() == "S")
                    {
                        var message = new SignalRMessageDto
                        {
                            String1 = "String1 from Server",
                            String2 = "String2 from Server"
                        };
                        _myHub.SendSignalRMessageDto(message);
                    }
                    if (key.ToUpper() == "R")
                    {
                        _myHub.RequestSpool();
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
                        Console.WriteLine("R - request spool");
                        Console.WriteLine("C - close application");
                        Console.WriteLine("----------------------");
                    }
                }

                Console.ReadLine();
            }
        }

        public void Configuration(IAppBuilder app)
        {
            // Branch the pipeline here for requests that start with "/signalr"
            app.Map("/signalr", map =>
            {
                // Setup the CORS middleware to run before SignalR.
                // By default this will allow all origins. You can 
                // configure the set of origins and/or http verbs by
                // providing a cors options with a different policy.
                map.UseCors(CorsOptions.AllowAll);
                var hubConfiguration = new HubConfiguration
                {
                    EnableDetailedErrors = true 
                    // You can enable JSONP by uncommenting line below.
                    // JSONP requests are insecure but some older browsers (and some
                    // versions of IE) require JSONP to work cross domain
                    // EnableJSONP = true
                };
                // Run the SignalR pipeline. We're not using MapSignalR
                // since this branch already runs under the "/signalr"
                // path.

                map.RunSignalR(hubConfiguration);
            });
        }
    }
}