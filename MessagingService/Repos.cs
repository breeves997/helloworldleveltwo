using MessagingService.Contracts;
using MongoDB.Bson;
using MongoDB.Driver;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MessagingService
{
    public abstract class SimpleMongoRepo<T> : IRepository<T>
    {
        //obviosly do not save your connection strings in plain text on your classes
        private const string _cxn = "mongodb://admin:servicesarecool@ds011785.mlab.com:11785/qbsol-messaging";
        protected static MongoClient _client;
        private static bool __isInitialized = false;
        protected IMongoDatabase _db;
        protected IMongoCollection<T> _collection;

        public static void Initialize()
        {
            _client = new MongoClient(_cxn);
            __isInitialized = true;
        }


        public SimpleMongoRepo(string collection)
        {
            if (!__isInitialized) Initialize();
            _db = _client.GetDatabase("qbsol-messaging");
            _collection = _db.GetCollection<T>(collection);
        }

        public abstract List<T> Find(T filter);

        public abstract T FindOne(T filter);

        public abstract T Save(T item);
    }
    public class ContactsRepo : SimpleMongoRepo<Contact>
    {
        public ContactsRepo() : base("contacts")
        {
        }
        public override List<Contact> Find(Contact filter)
        {
            var mfilter = GetFilter(filter);
            return _collection.Find(mfilter).ToList();
        }

        public override Contact FindOne(Contact filter)
        {
            return Find(filter).FirstOrDefault();
        }

        public override Contact Save(Contact item)
        {
            try
            {
                if (FindOne(item) == default(Contact))
                {
                    _collection.InsertOne(item);
                }
                else
                {
                    var filter = GetFilter(item);
                    var updateDef = Builders<Contact>.Update.Set(t => t.EmailAddress, item.EmailAddress);
                    updateDef.Set(t => t.PhoneNumber, item.PhoneNumber);
                    updateDef.Set(t => t.FavouriteProgrammingLanguage, item.FavouriteProgrammingLanguage);
                    _collection.FindOneAndUpdate(filter, updateDef);
                }
                return item;
            }
            catch
            {
                return null;
            }
        }
        private BsonDocument GetFilter(Contact filter)
        {
            var mfilter = new BsonDocument();
            if(!String.IsNullOrEmpty(filter.Name))
                mfilter.AddRange(new Dictionary<string, object>() { [nameof(filter.Name)] = filter.Name });
            if(!String.IsNullOrEmpty(filter.FavouriteProgrammingLanguage))
                mfilter.AddRange(new Dictionary<string, object>() { [nameof(filter.FavouriteProgrammingLanguage)] = filter.FavouriteProgrammingLanguage });
            return mfilter;
        }
    }

    public class MessagesRepo : SimpleMongoRepo<Message>
    {
        public MessagesRepo() : base("messages")
        { }

        public override List<Message> Find(Message filter)
        {
            var mfilter = GetFilter(filter);
            return _collection.Find(mfilter).ToList();
        }

        public override Message FindOne(Message filter)
        {
            return Find(filter).FirstOrDefault();
        }

        public override Message Save(Message item)
        {
            try
            {
                if (FindOne(item) == default(Message))
                {
                    _collection.InsertOne(item);
                }
                else
                {
                    //cannot update a message
                    return null;
                }
                return item;
            }
            catch
            {
                return null;
            }
        }
        private BsonDocument GetFilter(Message filter)
        {
            var mfilter = new BsonDocument();
            if(!String.IsNullOrEmpty(filter.From))
                mfilter.AddRange(new Dictionary<string, object>() { [nameof(filter.From)] = filter.From });
            if(!String.IsNullOrEmpty(filter.To))
                mfilter.AddRange(new Dictionary<string, object>() { [nameof(filter.To)] = filter.To });
            return mfilter;
        }
    }
}
