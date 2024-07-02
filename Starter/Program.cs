// See https://aka.ms/new-console-template for more information
using Starter;


//Task.Run(() =>  GRPC_Gateway.Class1.Main());

ServerWeb.WebServer.Start(null);

//FrontEnd.Server.pro

OPC_UA_Client client = new OPC_UA_Client();
client.Start();
OPC_UA_Server server = new OPC_UA_Server();
server.Start();

do
{

    //Console.WriteLine("Ciclo");
    Thread.Sleep(1000);
} while (true);