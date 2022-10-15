﻿using MongoDB.Bson;
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

    }
}
