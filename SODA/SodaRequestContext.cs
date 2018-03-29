using System;
using System.Collections.Generic;

namespace SODA
{
    /// <summary>
    /// Context object representing details of a request to a Socrata host.
    /// </summary>
    public class SodaRequestContext
    {
        /// <summary>
        /// The complete Uri of the request
        /// </summary>
        public Uri Uri { get; set; }
        /// <summary>
        /// One of the <see cref="SodaDataFormat"/> supported by Socrata.
        /// </summary>
        public SodaDataFormat DataFormat { get; set; }
        /// <summary>
        /// The HTTP Method of the request.
        /// </summary>
        public string Method { get; set; }
        /// <summary>
        /// The Socata App Token to use for the request.
        /// </summary>
        public string AppToken { get; set; }
        /// <summary>
        /// The Socrata username to use for the request.
        /// </summary>
        public string Username { get; set; }
        /// <summary>
        /// The plaintext password associated with the <see cref="Username"/>.
        /// </summary>
        public string Password { get; set; }
        /// <summary>
        /// An optional timeout override for the underlying web request.
        /// </summary>
        public int? Timeout { get; set; }
        /// <summary>
        /// An optional dictionary of headers to add to the request.
        /// </summary>
        public IDictionary<string, string> AdditionalHeaders { get; set; }

        /// <summary>
        /// Initialize a new <see cref="SodaRequestContext"/>.
        /// </summary>
        public SodaRequestContext()
        {
            AdditionalHeaders = new Dictionary<string, string>();
            DataFormat = SodaDataFormat.JSON;
        }
    }

    /// <summary>
    /// A <see cref="SodaRequestContext"/> typed for a particular payload.
    /// </summary>
    /// <typeparam name="TPayload">The type of the payload for the request.</typeparam>
    public class SodaRequestContext<TPayload> : SodaRequestContext
    {
        /// <summary>
        /// The payload data to send with the request.
        /// </summary>
        public TPayload Payload { get; set; }
    }
}
