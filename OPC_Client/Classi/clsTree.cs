using Opc.Ua;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OPC_Client.Classi
{
    public class clsTree
    {

        public string key { get; set; }
        public clsTreeData data { get; set; }
        public clsTree[]? children { get; set; }



        public clsTree(string key=null, ReferenceDescription rd= null )
        {
            data= new clsTreeData();
            if (rd is object)
            {
                this.key = key;
                data.DisplayName = rd.DisplayName.Text;
                data.BrowseName = rd.BrowseName.Name;
                data.NodeClass = rd.NodeClass.ToString();
                data.NodeId = rd.NodeId.ToString();
                data.Identifier = rd.NodeId?.Identifier?.ToString();
                data.NamespaceIndex = rd.NodeId.NamespaceIndex.ToString();
                data.ServerIndex = rd.NodeId.ServerIndex.ToString();

             }
            else
            {
                this.key = "";
                data.DisplayName = "root";
            }
        }

        public void AddChild(clsTree chiild)
        {
            List<clsTree> list ;
            if (children == null)
            {
                list = new List<clsTree>();
            }
            else 
            {
                list = children.ToList();
            }

            list.Add(chiild);

           children =list.ToArray();



        }

        //public clsTree(ReferenceDescription r)
        //{
        //    this.DisplayName = r.DisplayName.Text;
        //    this.BrowseName = r.BrowseName.Name;
        //    this.NamespaceIndex = r.NodeId.NamespaceIndex;
        //    this.NodeClass = r.NodeClass.ToString();
        //    this.NodeId = r.NodeId.ToString();
        //}
    }
}
