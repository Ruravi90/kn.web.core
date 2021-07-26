using System;
using System.Data;
using System.Runtime.Serialization;
using System.Threading.Tasks;
using MySqlConnector;

namespace kn.web.core.Soap
{
    [DataContract]
    public class Event
    {
        [DataMember]
        public string IMEI { get; set; }
        [DataMember]
        public int EventType { get; set; }
        [DataMember]
        public DateTime DTime { get; set; }
        [DataMember]
        public float Lat { get; set; }
        [DataMember]
        public float Lon { get; set; }
        [DataMember]
        public int Speed { get; set; }
        [DataMember]
        public string Address { get; set; }
        [DataMember]
        public string Plate { get; set; }
        [DataMember]
        public string Alias { get; set; }
        [DataMember]
        public float Course { get; set; }
    }
}