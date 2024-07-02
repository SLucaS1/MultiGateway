using Opc.Ua;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OPC_Client
{
    public class TagClass
    {
        public TagClass(string displayName, string opcAddress, string group, int server, int updateRate)
        {
            DisplayName = displayName;
            OPCAddress = opcAddress;
            Group = group;
            ServerID = server;
            UpdateRate = updateRate;
        }

        public DateTime LastUpdatedTime { get; set; }

        public DateTime LastSourceTimeStamp { get; set; }


        public uint StatusCode { get; set; }

        public object LastGoodValue { get; set; }
        public object CurrentValue { get; set; }
        public NodeId NodeID { get; set; }

        public int ServerID { get; set; }
        public string DisplayName { get; set; }
        public string OPCAddress { get; set; }
        public string Group { get; set; }
        public int UpdateRate { get; set; }

        public Type OPC_Type { get; set; } = null;
        public uint CounterRead { get; set; } = 0;
        public uint CounterWrite { get; set; } = 0;
    }
}
