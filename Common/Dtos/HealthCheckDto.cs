using System;
using System.Runtime.Serialization;
namespace Common.Dtos
{
    [DataContract]
    public class HealthCheckDto
    {
        [DataMember]
        public string Id { get; set; }
        [DataMember]
        public DateTime Timestamp { get; set; }
        [DataMember]
        public bool IsHealth { get; set; }
        [DataMember]
        public string ServiceName { get; set; }
    }
}
