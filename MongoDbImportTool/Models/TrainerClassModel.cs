﻿using MongoDB.Bson;
using System.Collections.Generic;
using MongoDbImportTool.Interfaces;

namespace MongoDbImportTool.Models
{
    /// <summary>
    /// Represents an trainer class in the TrainerClassDex
    /// </summary>
    public class TrainerClassModelv1 :  IDocument, IDexDocument
    {
        /// <inheritdoc/>
        public ObjectId _id { get; set; }

        /// <inheritdoc/>
        public string Name { get; set; }

        /// <summary>
        /// The Class' base class, if applicable
        /// </summary>
        public string BaseClass { get; set; }

        /// <summary>
        /// Whether the class is a base class
        /// </summary>
        public bool IsBaseClass { get; set; }

        /// <summary>
        /// The Features learned by the class
        /// </summary>
        public IEnumerable<TrainerClassFeatModelv1> Feats { get; set; }

        /// <summary>
        /// The Class' primary stat
        /// </summary>
        public string PrimaryStat { get; set; }

        /// <summary>
        /// The Class' secondary stat
        /// </summary>
        public string SecondaryStat { get; set; }

        /// <summary>
        /// The Skills granted by the Class
        /// </summary>
        public string Skills { get; set; }
    }
}
