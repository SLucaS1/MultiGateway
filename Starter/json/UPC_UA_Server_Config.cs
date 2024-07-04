using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Starter.json
{
    internal class UPC_UA_Server_Config
    {
        public string Address { get; set; }
        public string Username { get; set; }
        public string Password { get; set; }
        public Boolean SecurityEnabled { get; set; }
        public UPC_UA_Server_Obj[] Objs { get; set; }
        

    }
}
