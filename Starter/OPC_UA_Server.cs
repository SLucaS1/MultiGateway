using OPC_Server;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;
using System.Net;
using Opc.Ua.Configuration;
using Opc.Ua;
using Starter.json;
using System.Text.Json;

namespace Starter
{
    internal class OPC_UA_Server
    {
        Server server;

        public OPC_UA_Server()
        {
            string filePath = "config/OPC_US_Server.json";
            string jsonString = File.ReadAllText(filePath);
            UPC_UA_Server_Config config = JsonSerializer.Deserialize<UPC_UA_Server_Config>(jsonString);



            Server.Address = config.Address;
        }

        internal void Start()
        {
            Server.Main ();
        }
    }


}
