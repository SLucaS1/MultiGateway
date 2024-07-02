using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OPC_Client
{
    public class Group
    {
        public string Name = "";
        public int UpdateRate = 1000;

        public List<TagClass> Tags = new List<TagClass>();

    }
}
