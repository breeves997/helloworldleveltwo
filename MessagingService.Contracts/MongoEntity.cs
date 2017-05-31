using MongoDB.Bson;
using System.Runtime.Serialization;

namespace MessagingService.Contracts
{
    [DataContract]
    public abstract class MongoEntity
    {
        /// <summary>
        /// In real life, we shouldn't be introducing a dependency on an implementation detail
        /// into our C# POCOs! But guess what, this isn't real life so we frankly my dear, I don't give a damn
        /// </summary>
        [DataMember]
        public ObjectId _id { get; set; }
    }
}