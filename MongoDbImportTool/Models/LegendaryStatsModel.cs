﻿using System;
using System.Collections.Generic;

namespace MongoDbImportTool.Models
{
    /// <summary>
    /// Represents that Stats attributed for Legendary Pokemon
    /// </summary>
    public class LegendaryStatsModelv1
    {
        /// <summary>
        /// The Legendary HP stat
        /// </summary>
        public int HP { get; set; }

        /// <summary>
        /// The Additional Moves
        /// </summary>
        public IEnumerable<string> Moves { get; set; }

        /// <summary>
        /// The Legendary Moves
        /// </summary>
        public IEnumerable<string> LegendaryMoves { get; set; }

        /// <summary>
        /// The Legendary Passives
        /// </summary>
        public IEnumerable<string> Passives { get; set; }

        /// <summary>
        /// The Legendary Features
        /// </summary>
        public IEnumerable<string> Features { get; set; }
    }
}
