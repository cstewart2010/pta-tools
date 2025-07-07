﻿using MongoDB.Bson;
using System;
using System.Collections.Generic;
using MongoDbImportTool.Interfaces;

namespace MongoDbImportTool.Models
{
    /// <summary>
    /// Represents a shop in a PTA session
    /// </summary>
    public class ShopModelv1 : IDocument
    {
        /// <inheritdoc/>
        public ObjectId _id { get; set; }

        /// <summary>
        /// The shop's id
        /// </summary>
        public Guid ShopId { get; set; }

        /// <summary>
        /// The game's id
        /// </summary>
        public Guid GameId { get; set; }

        /// <summary>
        /// The shop's name
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// Whether the shop is active for trainers to visit
        /// </summary>
        public bool IsActive { get; set; }

        /// <summary>
        /// A collection of items on sale
        /// </summary>
        public Dictionary<string, WareModelv1> Inventory { get; set; }
    }
}
