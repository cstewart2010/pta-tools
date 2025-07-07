﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MongoDbImportTool.Models;

namespace MongoDbImportTool.Enums
{
    /// <summary>
    /// Container for all possible <see cref="SettingModelv1"/> types
    /// </summary>
    public enum SettingType
    {
        /// <summary>
        /// Represents a Hostile Setting 
        /// </summary>
        Hostile = 1,

        /// <summary>
        /// Represents a NonHostile Setting 
        /// </summary>
        NonHostile = 2,

        /// <summary>
        /// Represents an Setting with both Wild and Trainer participants
        /// </summary>
        Hybrid = 3
    }
}
