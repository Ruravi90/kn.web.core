using System;
using System.Data;
using System.Runtime.Serialization;
using System.Threading.Tasks;
using MySqlConnector;

namespace kn.web.core.Models
{
    [DataContract]
    public class Event
    {
        public int Id { get; set; }
        [DataMember]
        public string antena { get; set; }
        [DataMember]
        public string plataforma { get; set; }
        [DataMember]
        public DateTime fecha_evento { get; set; }
        [DataMember]
        public DateTime fecha_recepcion { get; set; }
        [DataMember]
        public string posicion { get; set; }
        [DataMember]
        public float latitud { get; set; }
        [DataMember]
        public float longitud { get; set; }
        [DataMember]
        public int tipo_evento { get; set; }
        [DataMember]
        public string info_adicional { get; set; }
        [DataMember]
        public float ignicion { get; set; }
    }
}