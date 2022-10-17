using MongoDB.Bson;
using MongoDB.Driver;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GamePlan
{
    static class DbController
    {
        public static IMongoDatabase db = null;

        public static void InitDb()
        {
            if (db == null)
            {
                var settings = MongoClientSettings.FromConnectionString(ConnectionString.getConnectionString());
                settings.ServerApi = new ServerApi(ServerApiVersion.V1);
                var client = new MongoClient(settings);
                db = client.GetDatabase("gameplan");
            }
        }
        public static List<BsonDocument> GetAll()
        {
            var collection = db.GetCollection<BsonDocument>("game");
            var games = collection.Find(_ => true).ToList();
            return games;
        }

        public static BsonDocument GetOne(string title)
        {
            var collection = db.GetCollection<BsonDocument>("game");
            var games = collection.Find(_ => true).ToList();
            BsonDocument game = null;
            foreach (var g in games)
            {
                if(g.GetValue("title") == title)
                {
                    game = g;
                }
            }
            return game;
        }

        internal static void UpdateNote(string title, string newBody)
        {
            var collection = db.GetCollection<BsonDocument>("game");
            collection.UpdateOne("{'title':'" + title + "'}",
            "{$set: {'notes':'" + newBody + "'}}"
            );
        }

        internal static void UpdateGameTitle(string oldTitle, string newTitle)
        {
            var collection = db.GetCollection<BsonDocument>("game");
            collection.UpdateOne("{'title':'" + oldTitle + "'}",
            "{$set: {'title':'" + newTitle + "'}}"
            );
        }

        internal static void InsertOne(string title)
        {
            var collection = db.GetCollection<BsonDocument>("game");
            BsonDocument newGame = new BsonDocument();
            BsonElement element1 = new BsonElement("title", title);
            BsonElement element2 = new BsonElement("notes", "");
            newGame.Add(element1);
            newGame.Add(element2);
            collection.InsertOne(newGame);
        }

        internal static void DeleteOne(string title)
        {
            var collection = db.GetCollection<BsonDocument>("game");
            collection.DeleteOne("{'title':'" + title + "'}");
        }
    }
}
