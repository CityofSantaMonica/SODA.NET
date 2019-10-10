using System.Collections.Generic;
using System;
namespace SODA
{
    /// <summary>
    /// A class for applying transforms to the schema programmatically.
    /// </summary>
    public class SchemaTransforms
    {
        /// <summary>
        /// A class for interacting with Socrata Data Portals using the Socrata Open Data API.
        /// </summary>
        Source source;

        /// <summary>
        /// A class for interacting with Socrata Data Portals using the Socrata Open Data API.
        /// </summary>
        public SchemaTransforms(Source source)
        {
            this.source = source;
        }
        /// <summary>
        /// A class for interacting with Socrata Data Portals using the Socrata Open Data API.
        /// </summary>
        public void ChangeColumnFieldname()
        {

        }
        /// <summary>
        /// A class for interacting with Socrata Data Portals using the Socrata Open Data API.
        /// </summary>
        public void ChangeColumnDescription()
        {

        }
        /// <summary>
        /// A class for interacting with Socrata Data Portals using the Socrata Open Data API.
        /// </summary>
        public void ChangeColumnDisplayName()
        {

        }
        /// <summary>
        /// A class for interacting with Socrata Data Portals using the Socrata Open Data API.
        /// </summary>
        public void ChangeColumnTransform()
        {

        }
        /// <summary>
        /// A class for interacting with Socrata Data Portals using the Socrata Open Data API.
        /// </summary>
        public void AddColumn()
        {

        }
        /// <summary>
        /// A class for interacting with Socrata Data Portals using the Socrata Open Data API.
        /// </summary>
        public Source GetSource()
        {
            return this.source;
        }
        /// <summary>
        /// A class for interacting with Socrata Data Portals using the Socrata Open Data API.
        /// </summary>
        public string GetOutputSchemaId()
        {
            return this.source.GetSchemaId();
        }

        /// <summary>
        /// Apply the transforms; Currently a NO-OP.
        /// </summary>
        public AppliedTransform Run()
        {
            return new AppliedTransform(this.source);
        }

    }
}
