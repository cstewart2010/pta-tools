using MongoDB.Driver;
using System;
using MongoDbImportTool.Models;

namespace MongoDbImportTool.Internal
{
    internal static class MongoCollectionHelper
    {
        static MongoCollectionHelper()
        {
            var settings = GetMongoClientSettings();
            var client = new MongoClient(settings);
            var databaseName = Environment.GetEnvironmentVariable("Database", EnvironmentVariableTarget.Process);
            Database = client.GetDatabase(databaseName);
            Sprite = Database.GetCollection<SpriteModelv1>("Sprites");
        }

        /// <summary>
        /// Represents the BasePokemon Collection
        /// </summary>
        public static IMongoDatabase Database { get; }

        /// <summary>
        /// Represents the Sprites Collection
        /// </summary>
        public static IMongoCollection<SpriteModelv1> Sprite { get; }

        private static MongoClientSettings GetMongoClientSettings()
        {
            var connectionString = Environment.GetEnvironmentVariable("MongoDBConnectionString", EnvironmentVariableTarget.Process);
            if (string.IsNullOrWhiteSpace(connectionString))
            {
                throw new NullReferenceException("MongoDBConnectionString environment variable need to be set to access MongoDB");
            }

            var settings = MongoClientSettings.FromConnectionString(connectionString);
            settings.SslSettings = new SslSettings()
            {
                EnabledSslProtocols = System.Security.Authentication.SslProtocols.Tls12
            };

            return settings;
        }
    }
}
