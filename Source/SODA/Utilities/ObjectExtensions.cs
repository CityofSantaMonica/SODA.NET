using Newtonsoft.Json;

namespace SODA.Utilities
{
    internal static class ObjectExtensions
    {
        /// <summary>
        /// Serializes the target to a JSON string.
        /// </summary>
        public static string ToJsonString(this object target, bool indented = false)
        {
            if (!indented)
                return JsonConvert.SerializeObject(target);
            else
                return JsonConvert.SerializeObject(target, Formatting.Indented);
        }
    }
}
