using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace MessagingService.Contracts
{
    [DataContract]
    public class Message : MongoEntity
    {
        [DataMember]
        public string From { get; set; }
        [DataMember]
        public string To { get; set; }
        [DataMember]
        public string Subject { get; set; }
        [DataMember]
        public string MessageContent { get; set; }
        
    }
}
