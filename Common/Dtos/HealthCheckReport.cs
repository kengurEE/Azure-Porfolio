using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace Common.Dtos
{
    [DataContract]

    public class HealthCheckReport
    {
        [DataMember]
        public double Portfolio { get; set; }
        [DataMember]
        public double Notification { get; set; }
        [DataMember]
        public List<HealthCheckDto> HealthChecks { get; set; }
    }
}
