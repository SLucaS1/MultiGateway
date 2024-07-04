
namespace Quickstarts.DataAccessServer
{
    /// <summary>
    /// Defines the possible tag types
    /// </summary>
    public enum UnderlyingSystemTagType
    {
        /// <summary>
        /// The tag has no special characteristics.
        /// </summary>
        Normal = 0,

        /// <summary>
        /// The tag is an analog value with a high and low range.
        /// </summary>
        Analog = 1,

        /// <summary>
        /// The tag is a digital value with a true and false state.
        /// </summary>
        Digital = 2,

        /// <summary>
        /// The tag is a enumerated value with set of names states.
        /// </summary>
        Enumerated = 3
    }
}
