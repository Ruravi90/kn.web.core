using System;
using System.Data;
using System.Runtime.Serialization;
using System.Text.Json.Serialization;
using System.Threading.Tasks;
using MySqlConnector;

namespace kn.web.core.Models
{
    [DataContract]
    public class User
    {
        public int Id { get; set;}
        [DataMember]
        public string CorporateName { get; set; }
        [DataMember]
        public string UserName { get; set; }
        
        [DataMember]
        [JsonIgnore]
        public string Password { get; set; }
        [DataMember]
        public int IsAdmin { get; set; }
    }
}