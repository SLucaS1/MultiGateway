using Opc.Ua.Client;
using Opc.Ua.Configuration;
using Opc.Ua;
using System.Formats.Asn1;
using System.Text.RegularExpressions;
using OPC_Client.Classi;
using OPC_Client.Shared;

namespace OPC_Client
{
    public class Client
    {
        public event LogEventHandler LogEvent;
        public delegate void LogEventHandler(object sender, string message);

        public event ErrorEventHandler LogError;
        public delegate void ErrorEventHandler(object sender, string name, Exception ex);

        public delegate void EventStatusChangeHandler(object sender, string StatusVal);
        public event EventStatusChangeHandler StatusChange;

        public delegate void UpdateItemHandler(object sender, TagClass[] UpdateItem);
        public event UpdateItemHandler UpdateItem;


        public delegate void UpdateCountersHandler(object sender, int addRead, int addWrite, int addError);
        public event UpdateCountersHandler UpdateCounters;



        public delegate void UpdateTagsCountersHandler(object sender, int ValidTag, int Groups, int MissingTags);
        public event UpdateTagsCountersHandler UpdateTagsCounters;

        public delegate void UpdateMissingTagsHandler(object sender, List<TagClass> MissingTags);
        public event UpdateMissingTagsHandler UpdateMissingTags;





        string ServerAddress { get; set; }
        string ConnectionName { get; set; }
        public string AppName { get => serverConfig.Name; }
        public int ID { get => serverConfig.id; }
        Session OPCSession { get; set; }
        List<Subscription> Subscriptions;

        public Boolean ClientInStandBy;

        public Dictionary<string, TagClass> TagList { get; set; }
        public Dictionary<string, Group> Groups;

        public DateTime LastTimeOPCServerFoundAlive { get; set; }
        public bool ClassDisposing { get; set; }
        public bool InitialisationCompleted { get; set; }
        private Thread RenewerTHread { get; set; }

        private ServerConfig serverConfig;

        private Dictionary<string, Dictionary<uint, TagClass>> GroupDict = new Dictionary<string, Dictionary<uint, TagClass>>();

        public bool Connected { get => OPCSession.Connected; }
        string connectionName;

        public Client(string connectionName)
        {
            this.connectionName = connectionName;
        }


        public void inizialize(ServerConfig serverConfig, Dictionary<string, TagClass> taglist)
        {
            try
            {
                Shared_Vars.OPC_Status.Address= serverConfig.Address;
                this.serverConfig = serverConfig;
                ServerAddress = serverConfig.Address;
                ConnectionName = connectionName;
                TagList = taglist;

                foreach (var b in serverConfig.BaseAddress)
                    if (b.Item2 is object)
                        foreach (var item in TagList)
                            item.Value.OPCAddress = item.Value.OPCAddress.Replace(b.Item1, b.Item2);
                    else
                        foreach (var item in TagList)
                            item.Value.OPCAddress = item.Value.OPCAddress.Replace(b.Item1, "");


                Groups = new Dictionary<string, Group>();

                foreach (var tag in TagList.Values)
                {
                    if (!Groups.ContainsKey(tag.Group))
                        Groups.Add(tag.Group, new Group() { Name = tag.Group, UpdateRate = tag.UpdateRate });
                    tag.NodeID = new NodeId(tag.OPCAddress, serverConfig.spaceIndex);
                    Groups[tag.Group].Tags.Add(tag);
                }
                Subscriptions = new List<Subscription>();

                LastTimeOPCServerFoundAlive = DateTime.Now;

                Task.Run(() => { InitializeOPCUAClient(); });


                if (serverConfig.sessionRenewalSeconds > 0)
                {
                    RenewerTHread = new Thread(renewSessionThread);
                    RenewerTHread.Start();
                }
            }
            catch (Exception ex)
            {
                LogError?.Invoke(this, "client.inizialize ", ex);
            }
        }




        public void Browsing()
        {
            BrowseNode(OPCSession, ObjectIds.ObjectsFolder, 0, Shared_Vars.OPC_Tree, "0");
        }





        static void BrowseNode(Session session, NodeId nodeId, int indent, clsTree tree,string key)
        {
            if (indent > 10)
                return;

            ReferenceDescriptionCollection references;
            Byte[] continuationPoint;

            session.Browse(null, null, nodeId, 0u, BrowseDirection.Forward,
                ReferenceTypeIds.HierarchicalReferences, true, (uint)NodeClass.Object | (uint)NodeClass.Variable,
                out continuationPoint, out references);
            int cont = 0;
            foreach (var rd in references)
            {
                string mykey = key + "-" + cont.ToString();
                clsTree t = new clsTree(mykey, rd);
                cont++;
                tree.AddChild(t);
                // Browsing ricorsivo dei figli
                if (rd.NodeClass == NodeClass.Object || rd.NodeClass == NodeClass.Variable)
                {
                    BrowseNode(session, (NodeId)rd.NodeId, indent + 1,t, mykey) ;
                   
                }
            }
        }


        public void dispose()
        {

            ClassDisposing = true;
            try
            {
                ClassDisposing = true;
                OPCSession.Close();
                OPCSession.Dispose();
                OPCSession = null;
                RenewerTHread.Abort();

            }
            catch { }

        }

        private void renewSessionThread()
        {
            while (!ClassDisposing)
            {
                if (!ClientInStandBy && (DateTime.Now - LastTimeOPCServerFoundAlive).TotalSeconds > serverConfig.sessionRenewalSeconds)
                {

                    try
                    {
                        TagList.Values.ToList().ForEach(i => i.StatusCode = DataValue.Bad);
                        UpdateItem?.Invoke(this, TagList.Values.ToArray());
                        UpdateCounters?.Invoke(this, 0, 0, 1);
                        LogEvent?.Invoke(this, "Renewing Session ");

                        StatusChange?.Invoke(this, "Disconnected");
                        if (OPCSession is object)
                        {
                            OPCSession.Close();
                            OPCSession.Dispose();
                        }

                        InitializeOPCUAClient();
                    }
                    catch { }
                }
                Thread.Sleep(2000);
            }
        }


        public void InitializeOPCUAClient()
        {
            try
            {
                LastTimeOPCServerFoundAlive = DateTime.Now.AddMinutes(1);
                var config = new ApplicationConfiguration()
                {
                    ApplicationName = ConnectionName,
                    ApplicationUri = Utils.Format(@"urn:{0}:" + ConnectionName + "", ServerAddress),
                    ApplicationType = ApplicationType.Client,
                    SecurityConfiguration = new SecurityConfiguration
                    {
                        ApplicationCertificate = new CertificateIdentifier
                        {
                            StoreType = serverConfig.ApplicationCertificateType,
                            StorePath = serverConfig.ApplicationCertificatePath,
                            SubjectName = Utils.Format(@"CN={0}, DC={1}",
                            ConnectionName, ServerAddress)
                        },
                        AutoAcceptUntrustedCertificates = serverConfig.AutoAcceptUntrustedCertificates,
                        AddAppCertToTrustedStore = serverConfig.AddAppCertToTrustedStore,
                        MinimumCertificateKeySize = 1024,
                        RejectSHA1SignedCertificates = false,
                        //TrustedUserCertificates = new CertificateTrustList() { StoreType = "Directory", StorePath= "..\\certificate" },
                    },
                    TransportConfigurations = new TransportConfigurationCollection(),
                    TransportQuotas = new TransportQuotas { OperationTimeout = serverConfig.OperationTimeout },
                    ClientConfiguration = new ClientConfiguration { DefaultSessionTimeout = serverConfig.DefaultSessionTimeout },
                    TraceConfiguration = new TraceConfiguration()
                };


                config.Validate(ApplicationType.Client).GetAwaiter().GetResult();
                if (config.SecurityConfiguration.AutoAcceptUntrustedCertificates)
                    config.CertificateValidator.CertificateValidation += (s, e) =>
                    {
                        e.Accept = e.Error.StatusCode == StatusCodes.BadCertificateUntrusted;
                    };

                var application = new ApplicationInstance
                {
                    ApplicationName = ConnectionName,
                    ApplicationType = ApplicationType.Client,
                    ApplicationConfiguration = config,
                };


                UserIdentity useridentity = null;
                if (serverConfig.Username is object && serverConfig.Password is object)
                    useridentity = new UserIdentity(serverConfig.Username, serverConfig.Password);

                application.CheckApplicationInstanceCertificate(false, 2048).GetAwaiter().GetResult();

                EndpointDescription selectedEndpoint = CoreClientUtils.SelectEndpoint(ServerAddress, useSecurity: serverConfig.SecurityEnabled, discoverTimeout: 15000);
                OPCSession = Session.Create(config, new ConfiguredEndpoint(null, selectedEndpoint, EndpointConfiguration.Create(config)), false, ConnectionName, 60000, useridentity, null).GetAwaiter().GetResult();

                LogEvent?.Invoke(this, "Connected to  " + ServerAddress);

                List<TagClass> MissingTags = new List<TagClass>();

                ValidateTags(TagList.Values.ToList());

                //Rimuovo gli eventi prima di cancellarlo
                foreach (var g in Subscriptions)
                    g.MonitoredItems.ToList().ForEach(i => i.Notification -= OnTagValueChange);

                Subscriptions.Clear();
                GroupDict.Clear();

                int c_TagV = 0;

                foreach (var g in Groups.Values)
                {
                    Subscription subscription = new Subscription(OPCSession.DefaultSubscription);
                    subscription.DisplayName = g.Name;
                    subscription.PublishingEnabled = true;
                    subscription.PublishingInterval = g.UpdateRate;
                    subscription.MaxNotificationsPerPublish = serverConfig.MaxNotificationsPerPublish;
                    subscription.KeepAliveCount = serverConfig.KeepAliveCount;
                    subscription.LifetimeCount = serverConfig.LifetimeCount;
                    subscription.Priority = serverConfig.Priority;


                    var list = new List<MonitoredItem> { };
                    list.Add(new MonitoredItem(subscription.DefaultItem) { DisplayName = "ServerStatusCurrentTime", StartNodeId = "i=2258" });


                    foreach (TagClass td in g.Tags)
                        if (td.OPC_Type is object)
                        {
                            list.Add(new MonitoredItem(subscription.DefaultItem) { DisplayName = td.DisplayName, StartNodeId = td.NodeID });
                            c_TagV++;
                        }
                        else
                            MissingTags.Add(td);


                    list.ForEach(i => i.Notification += OnTagValueChange);

                    subscription.AddItems(list);

                    OPCSession.AddSubscription(subscription);
                    subscription.Create();

                    Subscriptions.Add(subscription);
                }

                LastTimeOPCServerFoundAlive = DateTime.Now;

                UpdateTagsCounters?.Invoke(this, c_TagV, Subscriptions.Count, MissingTags.Count);
                UpdateMissingTags?.Invoke(this, MissingTags);
                if (MissingTags.Count > 0)
                    LogEvent?.Invoke(this, "Missing " + MissingTags.Count + " OPC tags");

                StatusChange?.Invoke(this, "Connected");
                LogEvent?.Invoke(this, "Subscribe   " + TagList.Count + " tags in " + Subscriptions.Count + " groups ");
            }
            catch (Exception ex)
            {
                if (ex is ServiceResultException && ((ServiceResultException)ex).StatusCode == StatusCodes.BadSecurityChecksFailed)
                {
                    //Certificato richiesto
                    LogError?.Invoke(this, "client.InitializeOPCUAClient", ex);
                    StatusChange?.Invoke(this, "Bad Security Checks Failed");
                }
                else if (ex is ServiceResultException && ((ServiceResultException)ex).StatusCode == StatusCodes.BadSecureChannelClosed)
                {
                    //Certificato sbagliato
                    LogError?.Invoke(this, "client.InitializeOPCUAClient", ex);
                    StatusChange?.Invoke(this, "Bad Secure Channel Closed");
                }
                else if (ex is ServiceResultException && ((ServiceResultException)ex).StatusCode == StatusCodes.BadEncodingLimitsExceeded)
                {

                    //Richiesta troppo grande
                    LogError?.Invoke(this, "client.InitializeOPCUAClient", ex);
                    StatusChange?.Invoke(this, "Bad Encoding Limits Exceeded");
                }
                else
                {
                    LogError?.Invoke(this, "client.InitializeOPCUAClient", ex);
                    StatusChange?.Invoke(this, "Connection error");
                }
            }

        }

        private void ValidateTags(List<TagClass> values)
        {
            try
            {
                //Se maggiore si 10000 li suddiviso in chiamate più piccole
                if (values.Count > 1000)
                {
                    for (int i = 0; i < Math.Ceiling(values.Count / 1000.0d); i++)
                    {
                        int c;
                        if ((i + 1) * 1000 > values.Count)
                            c = values.Count % 1000;
                        else
                            c = 1000;

                        ValidateTags(values.GetRange(i * 1000, c));
                    }
                    return;
                }

                //Cerco il tipo degli oggetti  nell'OPC
                ReadValueIdCollection nodesToRead = new ReadValueIdCollection();

                foreach (var i in values)
                {
                    ReadValueId nodeToRead = new ReadValueId();
                    nodeToRead.NodeId = i.NodeID;
                    nodeToRead.AttributeId = Attributes.Value;
                    nodesToRead.Add(nodeToRead);
                }

                DataValueCollection results = null;
                DiagnosticInfoCollection diagnosticInfos = null;

                ResponseHeader Response = OPCSession.Read(
                    null, 0,
                    TimestampsToReturn.Neither, nodesToRead,
                    out results,
                    out diagnosticInfos);

                ClientBase.ValidateResponse(results, nodesToRead);
                ClientBase.ValidateDiagnosticInfos(diagnosticInfos, nodesToRead);

                for (int i = 0; i < results.Count; i++)
                {
                    if (results[i].Value is object)
                        values[i].OPC_Type = results[i].Value.GetType();
                    else
                        values[i].OPC_Type = null;
                }

            }
            catch (Exception ex)
            {
                LogError?.Invoke(this, "client.ValidateTags", ex);
            }
        }

        public void Write(TagClass[] tags)
        {
            List<string> TagsNames = new List<string>();

            try
            {
                WriteValueCollection valuesToWrite = new WriteValueCollection();
                foreach (var t in tags)
                {
                    t.CounterWrite++;
                    WriteValue valueToWrite = new WriteValue();
                    TagsNames.Add(t.DisplayName + ":" + t.CurrentValue.ToString());
                    valueToWrite.NodeId = t.NodeID;
                    valueToWrite.AttributeId = Attributes.Value;
                    valueToWrite.Value.Value = Convert.ChangeType(t.CurrentValue, t.OPC_Type);
                    valueToWrite.Value.StatusCode = t.StatusCode;
                    valueToWrite.Value.ServerTimestamp = DateTime.MinValue;
                    valuesToWrite.Add(valueToWrite);
                }

                StatusCodeCollection results;
                DiagnosticInfoCollection diagnosticInfos;

                ResponseHeader Response = OPCSession.Write(null, valuesToWrite, out results, out diagnosticInfos);
                ClientBase.ValidateResponse(results, valuesToWrite);
                ClientBase.ValidateDiagnosticInfos(diagnosticInfos, valuesToWrite);

                int rOk = 0;
                int rFail = 0;
                foreach (var item in results)
                    if (item.Code == 0)
                        rOk++;
                    else
                        rFail++;

                UpdateCounters?.Invoke(this, 0, rOk, rFail);

            }
            catch (Exception ex)
            {
                LogError?.Invoke(this, "client.Write " + string.Join(";", TagsNames) + " ", ex);
                UpdateCounters?.Invoke(this, 0, 0, tags.Length);
            }
        }

        public Type getType(NodeId node)
        {
            try
            {
                ReadValueId nodeToRead = new ReadValueId();
                nodeToRead.NodeId = node;
                nodeToRead.AttributeId = Attributes.Value;

                ReadValueIdCollection nodesToRead = new ReadValueIdCollection();
                nodesToRead.Add(nodeToRead);

                DataValueCollection results = null;
                DiagnosticInfoCollection diagnosticInfos = null;

                ResponseHeader Response = OPCSession.Read(
                    null, 0,
                    TimestampsToReturn.Neither, nodesToRead,
                    out results,
                    out diagnosticInfos);

                ClientBase.ValidateResponse(results, nodesToRead);
                ClientBase.ValidateDiagnosticInfos(diagnosticInfos, nodesToRead);

                return results[0].Value.GetType();
            }
            catch (Exception ex)
            {
                LogError?.Invoke(this, "client.getType", ex);
                return null;
            }
        }


        private void extractTags(Session Session, NodeId NodeId, ref Dictionary<string, NodeId> dict, string baseAddress)
        {
            try
            {
                BrowseDescription nodeToBrowse = new BrowseDescription();
                nodeToBrowse.NodeId = NodeId;
                nodeToBrowse.BrowseDirection = BrowseDirection.Forward;
                nodeToBrowse.ReferenceTypeId = ReferenceTypeIds.Organizes;
                nodeToBrowse.IncludeSubtypes = true;
                nodeToBrowse.NodeClassMask = (uint)(NodeClass.Object | NodeClass.Variable);
                nodeToBrowse.ResultMask = (uint)BrowseResultMask.All;

                BrowseDescriptionCollection nodesToBrowse = new BrowseDescriptionCollection();
                nodesToBrowse.Add(nodeToBrowse);

                BrowseResultCollection results = null;
                DiagnosticInfoCollection diagnosticInfos = null;

                Session.Browse(null, null,
                            0, nodesToBrowse,
                            out results,
                            out diagnosticInfos);

                ClientBase.ValidateResponse(results, nodesToBrowse);
                ClientBase.ValidateDiagnosticInfos(diagnosticInfos, nodesToBrowse);

                foreach (var item in results)
                    foreach (var r in item.References)
                        if (!dict.ContainsKey(baseAddress + r.DisplayName))
                        {
                            dict.Add(baseAddress + r.DisplayName, (NodeId)r.NodeId);
                            extractTags(Session, (NodeId)r.NodeId, ref dict, r.DisplayName + ".");
                        }

            }
            catch (Exception ex)
            {
                LogError?.Invoke(this, "client.extractTags", ex);
            }
        }

        public void OnTagValueChange(MonitoredItem item, MonitoredItemNotificationEventArgs e)
        {
            try
            {
                List<TagClass> UpdateTag = new List<TagClass>();

                foreach (var value in item.DequeueValues())
                {

                    if (item.DisplayName == "ServerStatusCurrentTime")
                    {
                        LastTimeOPCServerFoundAlive = DateTime.Now;
                    }
                    else
                    {
                        if (TagList.ContainsKey(item.DisplayName))
                        {
                            if (value.Value != null)
                                TagList[item.DisplayName].LastGoodValue = value.Value;

                            TagList[item.DisplayName].CurrentValue = value.Value;
                            TagList[item.DisplayName].LastUpdatedTime = DateTime.Now;
                            TagList[item.DisplayName].LastSourceTimeStamp = value.SourceTimestamp.ToLocalTime();
                            TagList[item.DisplayName].StatusCode = value.StatusCode.Code;
                            TagList[item.DisplayName].CounterRead++;
                            UpdateTag.Add(TagList[item.DisplayName]);
                        }
                    }
                }

                if (UpdateTag.Count > 0)
                {
                    UpdateCounters?.Invoke(this, UpdateTag.Count, 0, 0);
                    UpdateItem?.Invoke(this, UpdateTag.ToArray());
                }

                InitialisationCompleted = true;
            }
            catch (Exception ex)
            {
                LogError?.Invoke(this, "client.OnTagValueChange", ex);
            }
        }




        public void UpdateSTBY(Boolean inSTBY)
        {
            try
            {

                if (inSTBY)
                {
                    foreach (var g in Subscriptions)
                        g.MonitoredItems.ToList().ForEach(i => i.Notification -= OnTagValueChange);

                    Subscriptions.Clear();
                    OPCSession.Dispose();
                    StatusChange?.Invoke(this, "Stand By");

                }
                else
                {
                    if (OPCSession == null || OPCSession.Disposed)
                        InitializeOPCUAClient();

                }
                ClientInStandBy = inSTBY;



            }
            catch (Exception ex)
            {
                LogError?.Invoke(this, "client.UpdateSTBY", ex);
            }

        }
    }

}
