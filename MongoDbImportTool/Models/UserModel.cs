﻿using MongoDB.Bson;
using System;
using System.Collections.Generic;
using MongoDbImportTool.Enums;
using MongoDbImportTool.Interfaces;
using MongoDbImportTool.Utilities;

namespace MongoDbImportTool.Models
{
    /// <summary>
    /// Represents a User in Pokemon Tabletop Adventures 
    /// </summary>
    public class UserModelv1 : IAuthenticated, IDocument
    {
        /// <summary>
        /// The default constructor for the MongoDB Csharp Driver
        /// </summary>
        public UserModelv1() { }

        /// <summary>
        /// Initializes a new instance of <see cref="UserModelv1"/>
        /// </summary>
        /// <param name="username"></param>
        /// <param name="password"></param>
        public UserModelv1(string username, string password)
        {
            UserId = Guid.NewGuid();
            Username = username;
            PasswordHash = EncryptionUtility.HashSecret(password);
            IsOnline = true;
            ActivityToken = EncryptionUtility.GenerateToken();
            DateCreated = DateTime.UtcNow.ToString("u");
            SiteRole = UserRoleOnSite.Active.ToString();
            Games = Array.Empty<Guid>();
            Messages = Array.Empty<Guid>();
        }

        /// <inheritdoc />
        public ObjectId _id { get; set; }

        /// <summary>
        /// Id for PTA user
        /// </summary>
        public Guid UserId { get; set; }

        /// <summary>
        /// Username for PTA user
        /// </summary>
        public string Username { get; set; }

        /// <inheritdoc />
        public bool IsOnline { get; set; }

        /// <inheritdoc />
        public string PasswordHash { get; set; }

        /// <summary>
        /// The 30 minute activity token for trainers
        /// </summary>
        public string ActivityToken { get; set; }

        /// <summary>
        /// Date PTA user account was created
        /// </summary>
        public string DateCreated { get; set; }

        /// <summary>
        /// Site role for PTA user
        /// </summary>
        public string SiteRole { get; set; }

        /// <summary>
        /// Games of which the PTA user is a member
        /// </summary>
        public ICollection<Guid> Games { get; set; }

        /// <summary>
        /// List of PTA user's messages
        /// </summary>
        public IEnumerable<Guid> Messages { get; set; }
    }
}
