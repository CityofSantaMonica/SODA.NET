using System;
using System.Runtime.Serialization;

namespace SODA.Discovery
{
    /// <summary>
    /// Information about a single result of a Discovery search.
    /// </summary>
    [DataContract]
    public class Result
    {
        /// <summary>
        /// Represents a dataset, visualization or other asset.
        /// </summary>
        [DataMember(Name = "resource")]
        public Resource Resource { get; internal set; }

        /// <summary>
        /// Describes the asset's classification by categories and tags.
        /// </summary>
        [DataMember(Name = "classification")]
        public Classification Classification { get; internal set; }

        /// <summary>
        /// Contains additional metadata about the asset.
        /// </summary>
        [DataMember(Name = "metadata")]
        public Metadata Metadata { get; internal set; }

        /// <summary>
        /// The permanent link of the asset
        /// </summary>
        [DataMember(Name = "permalink")]
        public Uri Permalink { get; internal set; }

        /// <summary>
        /// The prettier, but non-permanent link of the asset
        /// </summary>
        [DataMember(Name = "link")]
        public Uri Link { get; internal set; }

        /// <summary>
        /// The link to the preview image of the asset, if one is available.
        /// </summary>
        [DataMember(Name = "preview_image_url")]
        public Uri PreviewImageUrl { get; internal set; }

        /// <summary>
        /// Contains information about the asset's owner.
        /// </summary>
        [DataMember(Name = "owner")]
        public User Owner { get; internal set; }
    }
}
