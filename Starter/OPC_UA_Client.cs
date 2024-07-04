using OPC_Client;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;
using System.Net;
using Opc.Ua.Configuration;
using Opc.Ua;
using OPC_Client.Shared;
using OPC_Client.Classi;
using Starter.json;
using System.Text.Json;

namespace Starter
{
    internal class OPC_UA_Client
    {
        Client client; 
        UPC_UA_Client_Config config;
        Dictionary<string, clsTag> TagDict = new();

        public OPC_UA_Client()
        {
            string filePath = "config/OPC_US_Client.json";
            string jsonString = File.ReadAllText(filePath);
            UPC_UA_Client_Config config = JsonSerializer.Deserialize<UPC_UA_Client_Config>(jsonString);


            this.config = config;
        }

        public void Start()
        {
            Console.WriteLine("Start");

            client = new Client("Connessione");

            client.StatusChange += Client_StatusChange;
            client.LogEvent += Client_LogEvent;
            client.LogError += Client_LogError;
            client.UpdateItem += Client_UpdateItem;
            client.UpdateCounters += Client_UpdateCounters;
            client.UpdateTagsCounters += Client_UpdateTagsCounters; ;
            client.UpdateMissingTags += Client_UpdateMissingTags; ;

            ServerConfig serverConfigclient =new();
            serverConfigclient.Address = config.Address;
            serverConfigclient.Name = "Server";
            serverConfigclient.spaceIndex =3;

            //serverConfigclient.Username = config.Username;
            //serverConfigclient.Password = config.Password;
            //serverConfigclient.SecurityEnabled = config.SecurityEnabled ;

            Dictionary<string, TagClass> taglist= new Dictionary<string, TagClass>();
            taglist.Add("AI001", new TagClass("AI001", "AI001", "Gruppo", 0, 100));
            taglist.Add("DI001", new TagClass("DI001", "DI001", "Gruppo", 0, 100));


            //foreach (var i in OPCTags.Values)
            //    if (i.ServerID == serverConfig.Key)
            //        view.HUBTags.Add(i.DisplayName, i);



            //foreach (var i in Groups)
            //    if (i.ServerID == serverConfig.Key && GroupsDictCounter[i.Name] > 0)
            //        view.InfoGroup.Add(new vmInfoGroup() { Name = i.Name, UpdateRate = i.UpdateRate, TagNum = GroupsDictCounter[i.Name] }); ;

            client.inizialize(serverConfigclient, taglist);



        }

  

        private void Client_UpdateMissingTags(object sender, List<TagClass> MissingTags)
        {
        }

        private void Client_UpdateTagsCounters(object sender, int ValidTag, int Groups, int MissingTags)
        {
            client.Browsing();
            Console.WriteLine($"TagsCounters ValidTag {ValidTag}  MissingTags {MissingTags}" );

        }

        void Client_StatusChange(object sender, string StatusVal)
        {
            Console.ForegroundColor = ConsoleColor.Green;
            Console.WriteLine(StatusVal);
            Console.ForegroundColor = ConsoleColor.White;
            Shared_Vars.OPC_Status.Status= StatusVal;

        }
        void Client_LogEvent(object sender, string message)
        {
            Console.WriteLine(message);
        }
        void Client_LogError(object sender, string name, Exception ex)
        {
            Console.ForegroundColor = ConsoleColor.Red ;
            Console.WriteLine(name + " - " + ex.Message );
            Console.ForegroundColor = ConsoleColor.White ;
        }
        void Client_UpdateItem(object sender, TagClass[] UpdateItem)
        {
        }
        void Client_UpdateCounters(object sender, int addRead, int addWrite, int addError)
        {
            foreach (var tag in ((Client)sender).TagList )
            {
                Console.WriteLine( $"Update - {tag.Key} {tag.Value.CurrentValue}");

                if (!TagDict.ContainsKey(tag.Key))
                { 
                    clsTag t = new clsTag(tag.Key);
                    List<clsTag> tags = Shared_Vars.Tags.ToList();
                    tags.Add(t);
                    TagDict.Add(tag.Key, t);
                    Shared_Vars.Tags= tags.ToArray();
                }

                TagDict[tag.Key].Value = tag.Value.CurrentValue;
                TagDict[tag.Key].quality = OPC_Client.DataValue.QualityDescription( tag.Value.StatusCode);
                TagDict[tag.Key].CounterRead = tag.Value.CounterRead;
                TagDict[tag.Key].CounterWrite = tag.Value.CounterWrite;
                TagDict[tag.Key].UpdateRate = tag.Value.UpdateRate;
                TagDict[tag.Key].LastUpdatedTime = tag.Value.LastUpdatedTime.ToString("yyyy/MM/dd HH:mm:ss");

            }
        }
    }
}
