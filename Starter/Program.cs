// See https://aka.ms/new-console-template for more information
using Starter;
using Starter.json;
using System.Text.Json;


//ServerWeb.WebServer.Start(null);




OPC_UA_Client client = new OPC_UA_Client();
//client.Start();
OPC_UA_Server server = new OPC_UA_Server();
server.Start();

do
{

    //Console.WriteLine("Ciclo");
    Thread.Sleep(1000);
} while (true);