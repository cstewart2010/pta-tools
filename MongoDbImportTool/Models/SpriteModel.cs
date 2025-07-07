﻿using MongoDB.Bson;

namespace MongoDbImportTool.Models
{
    /// <summary>
    /// Represents a sprite in the Pokemon Tabletop adventures app
    /// </summary>
    public class SpriteModelv1
    {
        /// <summary>
        /// MongoDB id
        /// </summary>
        public ObjectId _id { get; set; }

        /// <summary>
        /// Friendly text for the select
        /// </summary>
        public string FriendlyText { get; set; }

        /// <summary>
        /// Value for the Pokemon Showdown sprite
        /// </summary>
        public string Value { get; set; }
    }
}
