
namespace Quickstarts.DataAccessServer
{
    /// <summary>
    /// Defines the possible tag data types
    /// </summary>
    public enum UnderlyingSystemDataType
    {
        /// <summary>
        /// A 1-byte integer value.
        /// </summary>
        Integer1 = 0,

        /// <summary>
        /// A 2-byte integer value.
        /// </summary>
        Integer2 = 1,

        /// <summary>
        /// A 4-byte integer value.
        /// </summary>
        Integer4 = 2,

        /// <summary>
        /// A 4-byte floating point value.
        /// </summary>
        Real4 = 3,

        /// <summary>
        /// A string value.
        /// </summary>
        String = 4
    }
}
