﻿using MongoDB.Bson;
using MongoDbImportTool.Interfaces;

namespace MongoDbImportTool.Models
{
    /// <summary>
    /// Represents an berry in the BerryDex
    /// </summary>
    public class BerryModelv1 : IDocument, IDexDocument
    {
        /// <inheritdoc/>
        public ObjectId _id { get; set; }

        /// <inheritdoc/>
        public string Name { get; set; }

        /// <summary>
        /// The berry's cost
        /// </summary>
        public int Price { get; set; }

        /// <summary>
        /// The effects of using the berry
        /// </summary>
        public string Effects { get; set; }

        /// <summary>
        /// The berry's flavors
        /// </summary>
        public string Flavors { get; set; }

        /// <summary>
        /// The berry's rarity
        /// </summary>
        public string Rarity { get; set; }
    }
}
