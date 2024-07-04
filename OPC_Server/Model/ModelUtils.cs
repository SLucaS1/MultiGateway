using System.Text;
using Opc.Ua;

namespace Quickstarts.DataAccessServer
{
    /// <summary>
    /// A class that builds NodeIds used by the DataAccess NodeManager
    /// </summary>
    public static class ModelUtils
    {
        /// <summary>
        /// The RootType for a Segment node identfier.
        /// </summary>
        public const int Segment = 0;

        /// <summary>
        /// The RootType for a Block node identfier.
        /// </summary>
        public const int Block = 1;

        /// <summary>
        /// Constructs a node identifier for a segment.
        /// </summary>
        /// <param name="segmentPath">The segment path.</param>
        /// <param name="namespaceIndex">Index of the namespace that qualifies the identifier.</param>
        /// <returns>The new node identifier.</returns>
        public static NodeId ConstructIdForSegment(string segmentPath, ushort namespaceIndex)
        {
            ParsedNodeId parsedNodeId = new ParsedNodeId();

            parsedNodeId.RootId = segmentPath;
            parsedNodeId.NamespaceIndex = namespaceIndex;
            parsedNodeId.RootType = 0;

            return parsedNodeId.Construct();
        }

        /// <summary>
        /// Constructs a NodeId for a block.
        /// </summary>
        /// <param name="blockId">The block id.</param>
        /// <param name="namespaceIndex">Index of the namespace.</param>
        /// <returns>The new NodeId.</returns>
        public static NodeId ConstructIdForBlock(string blockId, ushort namespaceIndex)
        {
            ParsedNodeId parsedNodeId = new ParsedNodeId();

            parsedNodeId.RootId = blockId;
            parsedNodeId.NamespaceIndex = namespaceIndex;
            parsedNodeId.RootType = 1;

            return parsedNodeId.Construct();
        }

        /// <summary>
        /// Constructs the node identifier for a component.
        /// </summary>
        /// <param name="component">The component.</param>
        /// <param name="namespaceIndex">Index of the namespace.</param>
        /// <returns>The node identifier for a component.</returns>
        public static NodeId ConstructIdForComponent(NodeState component, ushort namespaceIndex)
        {
            if (component == null)
            {
                return null;
            }

            // components must be instances with a parent.
            BaseInstanceState instance = component as BaseInstanceState;

            if (instance == null || instance.Parent == null)
            {
                return component.NodeId;
            }

            // parent must have a string identifier.
            string parentId = instance.Parent.NodeId.Identifier as string;
            
            if (parentId == null)
            {
                return null;
            }

            StringBuilder buffer = new StringBuilder();
            buffer.Append(parentId);
            
            // check if the parent is another component.
            int index = parentId.IndexOf('?');

            if (index < 0)
            {
                buffer.Append('?');
            }
            else
            {
                buffer.Append('/');
            }

            buffer.Append(component.SymbolicName);

            // return the node identifier.
            return new NodeId(buffer.ToString(), namespaceIndex);
        }
    }
}
