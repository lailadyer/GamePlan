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

        public static IMongoDatabase GetDb()
        {
            if (db == null)
            {
                var settings = MongoClientSettings.FromConnectionString(ConnectionString.getConnectionString());
                settings.ServerApi = new ServerApi(ServerApiVersion.V1);
                var client = new MongoClient(settings);
                db = client.GetDatabase("gameplan");
            }
            return db;
        }
        public static string GetAll()
        {
            string result = "";
            var collection = db.GetCollection<BsonDocument>("game");
            var games = collection.Find(_ => true).ToList();

            foreach (var game in games)
            {
                result += game.ToString();
            }
            return result;
        }

    }
}
