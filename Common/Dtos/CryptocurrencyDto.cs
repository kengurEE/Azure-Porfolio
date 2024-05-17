using System.Runtime.Serialization;

namespace Common.Dtos
{
    [DataContract]
    public class CryptocurrencyDto
    {
        [DataMember]
        public string Code { get; set; }
        [DataMember]
        public double Value { get; set; }
    }
}
