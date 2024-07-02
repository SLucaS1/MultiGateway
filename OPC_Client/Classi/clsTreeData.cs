using Opc.Ua;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OPC_Client.Classi
{
    public class clsTreeData
    {
        public string DisplayName { get; set; }
        public string BrowseName { get; set; }
        public string NodeClass { get; set; }

        public string NodeId { get; set; }
        public string Identifier { get; set; }
        public string NamespaceIndex { get; set; }
        public string ServerIndex { get; set; }


    }
}
