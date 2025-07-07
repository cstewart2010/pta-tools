﻿using MongoDB.Bson;
using MongoDbImportTool.Interfaces;

namespace MongoDbImportTool.Models
{
    /// <summary>
    /// Represents an item in the ItemDex
    /// </summary>
    public class BaseItemModelV1 : IDocument, IDexDocument
    {
        /// <inheritdoc/>
        public ObjectId _id { get; set; }

        /// <inheritdoc/>
        public string Name { get; set; }

        /// <summary>
        /// The item's cost
        /// </summary>
        public int Price { get; set; }

        /// <summary>
        /// The effects of using the item
        /// </summary>
        public string Effects { get; set; }
    }
}
