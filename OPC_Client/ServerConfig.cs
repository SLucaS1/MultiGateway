using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OPC_Client
{
    public class ServerConfig
    {
        public string Address;
        public string Name;
        public ushort spaceIndex = 3;
        public int id = 0;
        public uint sessionRenewalSeconds = 15;

        public bool SecurityEnabled = false;
        public bool AutoAcceptUntrustedCertificates = true;
        public bool AddAppCertToTrustedStore = false;
        public List<string> GroupPrefix = new List<string>();


        public string ApplicationCertificateType = "Directory";
        public string ApplicationCertificatePath = "certificate";
        //public string ApplicationCertificatePath = @"%CommonApplicationData%\OPC Foundation\CertificateStores\MachineDefault";

        public int OperationTimeout = 15000;
        public int DefaultSessionTimeout = 60000;
        public uint MaxNotificationsPerPublish = 0;
        public uint KeepAliveCount = 10;
        public uint LifetimeCount = 1000;
        public byte Priority = 255;

        public string Username;
        public string Password;

        public List<Tuple<string, string>> BaseAddress = new List<Tuple<string, string>>();

    }
}
