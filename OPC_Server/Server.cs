using System;
using System.Threading;
using System.Security.Cryptography.X509Certificates;
using Opc.Ua;
using Opc.Ua.Server;
using Opc.Ua.Configuration;
using Quickstarts.DataAccessServer;
using System.Net;

namespace OPC_Server
{
    public class Server
    {

        public static string Address;
        public static void Main()
        {
            Console.WriteLine("Starting OPC UA server...");
            Task.Run(() => StartServer()).Wait();
        }


        static async Task StartServer()
        {
            ApplicationInstance application = new ApplicationInstance
            {
                ApplicationName = "MyOPCUAServer",
                ApplicationType = ApplicationType.Server,
                ConfigSectionName = "MyOPCUAServer",
            };

            try
            {
                // process and command line arguments.
                if (application.ProcessCommandLine())
                {
                    return;
                }

                // check if running as a service.
                if (!Environment.UserInteractive)
                {
                    application.StartAsService(new DataAccessServer(Address));
                    return;
                }

                // load the application configuration.
                application.LoadApplicationConfiguration(false).Wait();

                // check the application certificate.
                application.CheckApplicationInstanceCertificate(false, 0).Wait();

                application.ApplicationConfiguration.ProductUri = Address;
                // start the server.
                application.Start(new DataAccessServer(Address)).Wait();

                // run the application interactively.
                //Task.Run(new ServerForm(application));


            }
            catch (Exception e)
            {

                return;
            }
        }



    }
}
