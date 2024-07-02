using Opc.Ua;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OPC_Client.Classi
{
    public class clsTag
    {
        public string Name { get; set; }
        public object Value { get; set; }
        public string quality { get; set; }
        public uint CounterRead { get; set; }
        public uint CounterWrite { get; set; }
        public int UpdateRate { get; set; }        
        public string LastUpdatedTime { get; set; }

  
        public clsTag(string name) { 
        this.Name = name;
        } 
    }
}
