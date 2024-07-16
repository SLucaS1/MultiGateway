using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OPC_Client.Shared
{
    public class Shared_Vars
    {
        public static Classi.clsTree OPC_Tree = new();

        public static Classi.clsStatus OPC_Status = new();

        public static Classi.clsTag[] Tags = [];


        public event LogEventHandler LogEvent;
        public delegate void LogEventHandler(object sender, string message);
    }
}
