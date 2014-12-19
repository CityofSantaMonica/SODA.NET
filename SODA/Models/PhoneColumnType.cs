namespace SODA.Models
{
    /// <summary>
    /// Enumeration of the phone types for a phone column.
    /// </summary>
    /// <remarks>
    /// Discovered by setting a phone column on the web site to each of the available items in the list, then pulling the dataset as JSON.
    /// </remarks>
    public enum PhoneColumnType
    {
        /// <summary>
        /// Value representing the type of the number is not set or unknown.
        /// </summary>
        Undefined,
        /// <summary>
        /// Value representing cell number.
        /// </summary>
        Cell,
        /// <summary>
        /// Value representing home number.
        /// </summary>
        Home,
        /// <summary>
        /// Value representing a work number.
        /// </summary>
        Work,
        /// <summary>
        /// Value representing a fax number.
        /// </summary>
        Fax,
        /// <summary>
        /// Value representing some other type of phone number.
        /// </summary>
        Other
    }
}
