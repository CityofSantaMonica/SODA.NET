using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using SODA.Utilities;

namespace SODA
{
    /// <summary>
    /// A class for Applying Transforms.
    /// </summary>
    public class AppliedTransform
    {
        Dictionary<string, dynamic> resource;
        public AppliedTransform(Dictionary<string, dynamic> transform)
        {

        }
        public string GetOutputSchemaId()
        {
            return this.resource["schemas"][0]["output_schemas"][0]["id"];
        }
    }
}
