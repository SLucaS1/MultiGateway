﻿using Opc.Ua.Server;
using Opc.Ua;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OPC_Server
{

        /// <summary>
        /// Implements a basic Quickstart Server.
        /// </summary>
        /// <remarks>
        /// Each server instance must have one instance of a StandardServer object which is
        /// responsible for reading the configuration file, creating the endpoints and dispatching
        /// incoming requests to the appropriate handler.
        /// 
        /// This sub-class specifies non-configurable metadata such as Product Name and initializes
        /// the EmptyNodeManager which provides access to the data exposed by the Server.
        /// </remarks>
        public partial class EmptyServer : StandardServer
        {
            #region Overridden Methods
            /// <summary>
            /// Creates the node managers for the server.
            /// </summary>
            /// <remarks>
            /// This method allows the sub-class create any additional node managers which it uses. The SDK
            /// always creates a CoreNodeManager which handles the built-in nodes defined by the specification.
            /// Any additional NodeManagers are expected to handle application specific nodes.
            /// </remarks>
            protected override MasterNodeManager CreateMasterNodeManager(IServerInternal server, ApplicationConfiguration configuration)
            {
                Utils.Trace("Creating the Node Managers.");

                List<INodeManager> nodeManagers = new List<INodeManager>();

                // create the custom node managers.
                nodeManagers.Add(new EmptyNodeManager(server, configuration));

                // create master node manager.
                return new MasterNodeManager(server, configuration, null, nodeManagers.ToArray());
            }

            /// <summary>
            /// Loads the non-configurable properties for the application.
            /// </summary>
            /// <remarks>
            /// These properties are exposed by the server but cannot be changed by administrators.
            /// </remarks>
            protected override ServerProperties LoadServerProperties()
            {
                ServerProperties properties = new ServerProperties();

                properties.ManufacturerName = "OPC Foundation";
                properties.ProductName = "Quickstart Empty Server";
                properties.ProductUri = "http://opcfoundation.org/Quickstart/EmptyServer/v1.0";
                properties.SoftwareVersion = Utils.GetAssemblySoftwareVersion();
                properties.BuildNumber = Utils.GetAssemblyBuildNumber();
                properties.BuildDate = Utils.GetAssemblyTimestamp();

                // TBD - All applications have software certificates that need to added to the properties.

                return properties;
            }
            #endregion
        }
    }
