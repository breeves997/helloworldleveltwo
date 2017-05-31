using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace MessagingService.Contracts
{
    [DataContract]
    public class Contact : MongoEntity
    {
        [DataMember]
       public string Name { get; set; } 
        [DataMember]
       public string EmailAddress { get; set; } 
        [DataMember]
       public string PhoneNumber { get; set; } 
        [DataMember]
       public string FavouriteProgrammingLanguage { get; set; } 
    }
}
