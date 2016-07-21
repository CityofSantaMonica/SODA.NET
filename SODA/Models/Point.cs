using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace SODA.Models
{
    [DataContract]
    class Point:Geometry
    {
        [DataMember]
        public new type type { get; set; }
        [DataMember]
        public double[] coordinates { get; set; }
    }
}
