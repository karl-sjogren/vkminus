using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;
using MongoDB.Bson.Serialization;
using MongoDB.Bson.Serialization.Options;
using MongoDB.Driver;
using MongoDB.Driver.Linq;
using Shorthand.VKMinus.Data.Models;

namespace Shorthand.VKMinus.Data
{
    public class Repository
    {
        private string _connectionString;
        private string ConnectionString
        {
            get
            {
                if (!string.IsNullOrEmpty(_connectionString))
                    return _connectionString;

                var doc = XDocument.Load("connection.xml");
                var node = doc.Descendants("connectionstring").FirstOrDefault();
                if (node == null)
                    return string.Empty;

                _connectionString = node.Value;
                return _connectionString;
            }
        }

        private readonly string _collection = "statistics";

        public Repository(string collection = "statistics") {
            _collection = collection;
            if (!BsonClassMap.IsClassMapRegistered(typeof(StartpageData)))
            {
                BsonClassMap.RegisterClassMap<StartpageData>(cm =>
                {
                    cm.AutoMap();
                    cm.SetIdMember(cm.GetMemberMap(c => c.Id));
                    cm.GetMemberMap(c => c.CreatedAt).SetSerializationOptions(new DateTimeSerializationOptions { Kind = DateTimeKind.Local });
                    cm.SetIgnoreExtraElements(true);
                });
                BsonClassMap.RegisterClassMap<BlockData>();


                BsonClassMap.RegisterClassMap<DailySummary>(cm =>
                {
                    cm.AutoMap();
                    cm.SetIdMember(cm.GetMemberMap(c => c.Id));
                    cm.GetMemberMap(c => c.CreatedAt).SetSerializationOptions(new DateTimeSerializationOptions { Kind = DateTimeKind.Local });
                    cm.SetIgnoreExtraElements(true);
                });
            }
        }

        public void Save(StartpageData data) {
            var client = new MongoClient(ConnectionString);
            var server = client.GetServer();
            var db = server.GetDatabase("vkminus");
            var coll = db.GetCollection(_collection);
            coll.Insert(data);
        }

        public void Save(DailySummary data)
        {
            var client = new MongoClient(ConnectionString);
            var server = client.GetServer();
            var db = server.GetDatabase("vkminus");
            var coll = db.GetCollection(_collection);
            coll.Insert(data);
        }

        public List<StartpageData> GetByDate(DateTime date) {
            var client = new MongoClient(ConnectionString);
            var server = client.GetServer();
            var db = server.GetDatabase("vkminus");
            var coll = db.GetCollection(_collection);
            coll.FindAs<StartpageData>(new QueryDocument());
            var items = coll.AsQueryable<StartpageData>().Where(o => o.CreatedAt >= date && o.CreatedAt < date.AddDays(1)).ToList();
            return items;
        }
    }
}
