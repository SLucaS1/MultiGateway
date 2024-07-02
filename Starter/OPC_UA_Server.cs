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

namespace Starter
{
    internal class OPC_UA_Server
    {
        Server server;

        public OPC_UA_Server()
        {
            server = new Server();
        }

        internal void Start()
        {
            server.start();
        }
    }


}
