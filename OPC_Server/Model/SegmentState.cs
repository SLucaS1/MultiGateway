using Opc.Ua;

namespace Quickstarts.DataAccessServer
{    
    /// <summary>
    /// A object which maps a segment to a UA object.
    /// </summary>
    public partial class SegmentState : FolderState
    {
        #region Constructors
        /// <summary>
        /// Initializes a new instance of the <see cref="SegmentState"/> class.
        /// </summary>
        /// <param name="context">The context.</param>
        /// <param name="nodeId">The node id.</param>
        /// <param name="segment">The segment.</param>
        public SegmentState(ISystemContext context, NodeId nodeId, UnderlyingSystemSegment segment) : base(null)
        {
            m_segmentPath = segment.Id;

            this.TypeDefinitionId = ObjectTypeIds.FolderType;
            this.SymbolicName = segment.Name;
            this.NodeId = nodeId;
            this.BrowseName = new QualifiedName(segment.Name, nodeId.NamespaceIndex);
            this.DisplayName = new LocalizedText(segment.Name);
            this.Description = null;
            this.WriteMask = 0;
            this.UserWriteMask = 0;
            this.EventNotifier = EventNotifiers.None;
        }
        #endregion

        #region Public Interface
        /// <summary>
        /// Gets the segment path.
        /// </summary>
        /// <value>The segment path.</value>
        public string SegmentPath
        {
            get { return m_segmentPath; }
        }

        /// <summary>
        /// Creates a browser that explores the structure of the block.
        /// </summary>
        /// <param name="context">The system context to use.</param>
        /// <param name="view">The view which may restrict the set of references/nodes found.</param>
        /// <param name="referenceType">The type of references being followed.</param>
        /// <param name="includeSubtypes">Whether subtypes of the reference type are followed.</param>
        /// <param name="browseDirection">Which way the references are being followed.</param>
        /// <param name="browseName">The browse name of a specific target (used when translating browse paths).</param>
        /// <param name="additionalReferences">Any additional references that should be included.</param>
        /// <param name="internalOnly">If true the browser should not making blocking calls to external systems.</param>
        /// <returns>The browse object (must be disposed).</returns>
        public override INodeBrowser CreateBrowser(
            ISystemContext context, 
            ViewDescription view, 
            NodeId referenceType, 
            bool includeSubtypes, 
            BrowseDirection browseDirection, 
            QualifiedName browseName,
            IEnumerable<IReference> additionalReferences,
            bool internalOnly)
        {
            NodeBrowser browser = new SegmentBrowser(
                context,
                view,
                referenceType,
                includeSubtypes,
                browseDirection,
                browseName,
                additionalReferences,
                internalOnly,
                this);

            PopulateBrowser(context, browser);

            return browser;
        }

        /// <summary>
        /// Populates the browser with references that meet the criteria.
        /// </summary>
        /// <param name="context">The context for the system being accessed.</param>
        /// <param name="browser">The browser to populate.</param>
        protected override void PopulateBrowser(ISystemContext context, NodeBrowser browser)
        {
            base.PopulateBrowser(context, browser);

            // check if the parent segments need to be returned.
            if (browser.IsRequired(ReferenceTypeIds.Organizes, true))
            {
                UnderlyingSystem system = context.SystemHandle as UnderlyingSystem;

                if (system == null)
                {
                    return;
                }

                // add reference for parent segment.
                UnderlyingSystemSegment segment = system.FindParentForSegment(m_segmentPath);

                if (segment != null)
                {
                    browser.Add(ReferenceTypeIds.Organizes, true, ModelUtils.ConstructIdForSegment(segment.Id, this.NodeId.NamespaceIndex));
                }
            }
        }
        #endregion

        #region Private Fields
        private string m_segmentPath;
        #endregion
    }
}
