using System.Runtime.Serialization;

namespace kn.web.core.Soap
{
    [DataContract]
    public class Filter
    {
        [DataMember]
        public string UserName { get; set; }
        [DataMember]
        public string Password { get; set; }
        [DataMember]
        public string Fecha_Ini { get; set; }
        [DataMember]
        public string Fecha_Fin { get; set; }
    }
}