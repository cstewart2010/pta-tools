﻿using MongoDB.Bson;
using System;
using System.Collections.Generic;
using MongoDbImportTool.Interfaces;

namespace MongoDbImportTool.Models
{
    /// <summary>
    /// Represents an encounter during a PTA session
    /// </summary>
    public class SettingModelv1 : IDocument
    {
        /// <inheritdoc/>
        public ObjectId _id { get; set; }

        /// <summary>
        /// The encounter's id
        /// </summary>
        public Guid SettingId { get; set; }

        /// <summary>
        /// The game id associated with the encounter
        /// </summary>
        public Guid GameId { get; set; }

        /// <summary>
        /// The encounter's friendly nickname
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// Whether or not the encounter is active for all trainers
        /// </summary>
        public bool IsActive { get; set; }

        /// <summary>
        /// The type of Setting 
        /// </summary>
        public string Type { get; set; }

        /// <summary>
        /// The settings participants
        /// </summary>
        public IEnumerable<SettingParticipantModelv1> ActiveParticipants { get; set; }

        /// <summary>
        /// The encounter's environment
        /// </summary>
        public string[] Environment { get; set; }


        /// <summary>
        /// The encounter's shops
        /// </summary>
        public IEnumerable<Guid> Shops { get; set; }
    }
}
