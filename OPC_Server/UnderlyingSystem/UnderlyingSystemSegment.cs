
namespace Quickstarts.DataAccessServer
{
    /// <summary>
    /// Stores information about a segment in the system.
    /// </summary>
    public class UnderlyingSystemSegment
    {
        #region Public Members
        /// <summary>
        /// Initializes a new instance of the <see cref="UnderlyingSystemSegment"/> class.
        /// </summary>
        public UnderlyingSystemSegment()
        {
        }
        #endregion

        #region Public Members
        /// <summary>
        /// Gets or sets the unique id for the segment.
        /// </summary>
        /// <value>The unique id for the segment</value>
        public string Id
        {
            get { return m_id; }
            set { m_id = value; }
        }

        /// <summary>
        /// Gets or sets the name of the segment.
        /// </summary>
        /// <value>The name of the segment.</value>
        public string Name
        {
            get { return m_name; }
            set { m_name = value; }
        }

        /// <summary>
        /// Gets or sets the type of the segment.
        /// </summary>
        /// <value>The type of the segment.</value>
        public string SegmentType
        {
            get { return m_segmentType; }
            set { m_segmentType = value; }
        }
        #endregion

        #region Private Methods
        #endregion

        #region Private Fields
        private string m_id;
        private string m_name;
        private string m_segmentType;
        #endregion
    }
}
