using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Threading.Tasks;

namespace kn.web.core.Models
{
    public class Filter
    {
        [DataMember]
        public string Fecha_Ini { get; set; }
        [DataMember]
        public string Fecha_Fin { get; set; }
    }
}
