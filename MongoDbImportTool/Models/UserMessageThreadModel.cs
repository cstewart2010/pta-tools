﻿using MongoDB.Bson;
using System;
using System.Collections.Generic;
using MongoDbImportTool.Interfaces;

namespace MongoDbImportTool.Models
{
    /// <summary>
    /// Represents a message thread in Pokemon Tabletop Adventures 
    /// </summary>
    public class UserMessageThreadModelv1 : IDocument
    {
        /// <inheritdoc />
        public ObjectId _id { get; set; }

        /// <summary>
        /// Id for PTA user Messages
        /// </summary>
        public Guid MessageId { get; set; }

        /// <summary>
        /// Collection of messages shared between two PTA Users
        /// </summary>
        public IEnumerable<UserMessageModelv1> Messages { get; set; }
    }
}
