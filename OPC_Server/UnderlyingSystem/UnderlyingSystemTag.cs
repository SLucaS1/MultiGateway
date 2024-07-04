
namespace Quickstarts.DataAccessServer
{
    /// <summary>
    /// This class stores the state of a tag known to the system.
    /// </summary>
    /// <remarks>
    /// This class only stores the information about an tag that a system has. The
    /// system has no concept of the UA information model and the NodeManager must 
    /// convert the information stored in this class into the UA equivalent.
    /// </remarks>
    public class UnderlyingSystemTag
    {
        #region Public Members
        /// <summary>
        /// The block that the tag belongs to
        /// </summary>
        /// <value>The block.</value>
        public UnderlyingSystemBlock Block
        {
            get { return m_block; }
            set { m_block = value; }
        }

        /// <summary>
        /// Gets or sets the name of the tag.
        /// </summary>
        /// <value>The name of the tag.</value>
        public string Name
        {
            get { return m_name; }
            set { m_name = value; }
        }

        /// <summary>
        /// Gets or sets the description of the tag.
        /// </summary>
        /// <value>The description of the tag.</value>
        public string Description
        {
            get { return m_description; }
            set { m_description = value; }
        }

        /// <summary>
        /// Gets or sets the engineering units for the tag.
        /// </summary>
        /// <value>The engineering units for the tag.</value>
        public string EngineeringUnits
        {
            get { return m_engineeringUnits; }
            set { m_engineeringUnits = value; }
        }

        /// <summary>
        /// Gets or sets the data type for the tag.
        /// </summary>
        /// <value>The data type for the tag.</value>
        public UnderlyingSystemDataType DataType
        {
            get { return m_dataType; }
            set { m_dataType = value; }
        }

        /// <summary>
        /// Gets or sets the type of the tag.
        /// </summary>
        /// <value>The type of the tag.</value>
        public UnderlyingSystemTagType TagType
        {
            get { return m_tagType; }
            set { m_tagType = value; }
        }

        /// <summary>
        /// Gets or sets the value of the tag.
        /// </summary>
        /// <value>The tag value.</value>
        public object Value
        {
            get { return m_value; }
            set { m_value = value; }
        }

        /// <summary>
        /// Gets or sets the timestamp for the value.
        /// </summary>
        /// <value>The timestamp for the value.</value>
        public DateTime Timestamp
        {
            get { return m_timestamp; }
            set { m_timestamp = value; }
        }

        /// <summary>
        /// Gets or sets a value indicating whether the value is writeable.
        /// </summary>
        /// <value>
        /// 	<c>true</c> if the vaklue is writeable; otherwise, <c>false</c>.
        /// </value>
        public bool IsWriteable
        {
            get { return m_isWriteable; }
            set { m_isWriteable = value; }
        }

        /// <summary>
        /// Gets or sets the EU ranges for the tag.
        /// </summary>
        /// <value>The EU ranges for the tag.</value>
        /// <remarks>
        /// 2 values: HighEU, LowEU
        /// 4 values: HighEU, LowEU, HighIR, LowIR
        /// </remarks>
        public double[] EuRange
        {
            get { return m_euRange; }
            set { m_euRange = value; }
        }

        /// <summary>
        /// Gets or sets the labels for the tag values.
        /// </summary>
        /// <value>The labels for the tag values.</value>
        /// <remarks>
        /// Digital Tags: TrueState, FalseState
        /// Enumerated Tags: Lookup table for Value.
        /// </remarks>
        public string[] Labels
        {
            get { return m_labels; }
            set { m_labels = value; }
        }

        /// <summary>
        /// Creates a snapshot of the tag.
        /// </summary>
        /// <returns>The snapshot.</returns>
        public UnderlyingSystemTag CreateSnapshot()
        {
            return (UnderlyingSystemTag)MemberwiseClone();
        }
        #endregion

        #region Private Fields
        private UnderlyingSystemBlock m_block;
        private string m_name;
        private string m_description;
        private string m_engineeringUnits;
        private UnderlyingSystemDataType m_dataType;
        private UnderlyingSystemTagType m_tagType;
        private bool m_isWriteable;
        private double[] m_euRange;
        private string[] m_labels;
        private object m_value;
        private DateTime m_timestamp;
        #endregion
    }
}
