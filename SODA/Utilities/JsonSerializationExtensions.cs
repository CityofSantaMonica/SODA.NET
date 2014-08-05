using Newtonsoft.Json;

namespace SODA.Utilities
{
    public static class JsonSerializationExtensions
    {
        /// <summary>
        /// Converts the target object into its JSON string representation.
        /// </summary>
        /// <returns>The serialized JSON string of the target object.</returns>
        public static string ToJsonString(this object target)
        {
            return JsonConvert.SerializeObject(target);
        }
    }
}
