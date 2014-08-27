using Newtonsoft.Json;

namespace SODA.Utilities
{
    /// <summary>
    /// Convenience methods for JSON serialization.
    /// </summary>
    public static class JsonSerializationExtensions
    {
        /// <summary>
        /// Converts the target object into its JSON string representation.
        /// </summary>
        /// <returns>The serialized JSON string of the target object.</returns>
        public static string ToJsonString(this object target)
        {
            return target == null ? null : JsonConvert.SerializeObject(target);
        }
    }
}
