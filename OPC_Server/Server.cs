using System;
using System.Threading;
using Opc.Ua;
using Opc.Ua.Configuration;
using Opc.Ua.Server;
using static System.Net.Mime.MediaTypeNames;

namespace OPC_Server
{
    public class Server
    {
        public void start()
        {

            //// Initialize the user interface.
            //Application.EnableVisualStyles();
            //Application.SetCompatibleTextRenderingDefault(false);

            //ApplicationInstance.MessageDlg = new ApplicationMessageDlg();
            ApplicationInstance application = new ApplicationInstance();
            application.ApplicationType = ApplicationType.Server;
            application.ConfigSectionName = "Quickstarts.EmptyServer";

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
                    application.StartAsService(new EmptyServer());
                    return;
                }

                // load the application configuration.
                application.LoadApplicationConfiguration(false).Wait();

                // check the application certificate.
                application.CheckApplicationInstanceCertificate(false, 0).Wait();

                // start the server.
                application.Start(new EmptyServer()).Wait();

            }
            catch (Exception e)
            {
                
                return;
            }
        }


    }
}
