// See https://aka.ms/new-console-template for more information
using Starter;
using Starter.json;
using System.Text.Json;


//ServerWeb.WebServer.Start(null);



string filePath = "config/OPC_US_Client.json";
string jsonString = File.ReadAllText(filePath);
UPC_UA_Client_Config UPC_UA_Client_Config = JsonSerializer.Deserialize<UPC_UA_Client_Config>(jsonString);

OPC_UA_Client client = new OPC_UA_Client(UPC_UA_Client_Config);
//client.Start();
OPC_UA_Server server = new OPC_UA_Server();
server.Start();

do
{

    //Console.WriteLine("Ciclo");
    Thread.Sleep(1000);
} while (true);