using MongoDB.Driver;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using MongoDbImportTool.Enums;
using MongoDbImportTool.Interfaces;
using MongoDbImportTool.Internal;
using MongoDbImportTool.Models;

namespace MongoDbImportTool.Utilities
{
    /// <summary>
    /// Provides a collection of methods for performing CRUD interactions on Dex Collections
    /// </summary>
    public static class DexUtility
    {
        /// <summary>
        /// Adds a collection of dex entry to a specific dex collection
        /// </summary>
        /// <param name="documents">The documents to add to collection</param>
        public static void AddPokedexEntries(IEnumerable<BasePokemonModelv1> documents)
        {
            var collection = MongoCollectionHelper.Database.GetCollection<BasePokemonModelv1>(nameof(DexType.BasePokemon));
            var currentDocuments = collection.Find(currentDocument => true).ToList();
            var uniqueDocs = documents.Where(x =>
                currentDocuments.All(currentDocument => x.Name != currentDocument.Name && x.Form != currentDocument.Form)).ToList();
            if (uniqueDocs.Count == 0)
            {
                Console.WriteLine($"No new {nameof(DexType.BasePokemon)} documents to add");
                return;
            }

            Console.WriteLine($"Adding new {nameof(DexType.BasePokemon)} documents");
            var isSuccessful = TryAddDexEntry
            (
                () => collection.InsertMany(uniqueDocs),
                out var error
            );

            if (!isSuccessful)
            {
                Console.WriteLine($"Failed to add all new {nameof(DexType.BasePokemon)} documents");
                Console.WriteLine(error.WriteErrorJsonString);
            }
        }

        /// <summary>
        /// Adds a collection of dex entry to a specific dex collection
        /// </summary>
        /// <param name="collectionName">The name of the collection to add document</param>
        /// <param name="documents">The documents to add to collection</param>
        /// <param name="writer">The writer to write logs to</param>
        public static void AddDexEntries<TDocument>(
            string collectionName,
            IEnumerable<TDocument> documents,
            TextWriter writer) where TDocument : IDexDocument
        {
            var collection = MongoCollectionHelper.Database.GetCollection<TDocument>(collectionName);
            var currentDocuments = collection.Find(currentDocument => true).ToList();
            var uniqueDocs = documents.Where(x =>
                currentDocuments.All(currentDocument => x.Name != currentDocument.Name)).ToList();
            if (uniqueDocs.Count == 0)
            {
                writer.WriteLine($"No new {collectionName} documents to add");
                return;
            }

            writer.WriteLine($"Adding new {collectionName} documents");
            var isSuccessful = TryAddDexEntry
            (
                () => collection.InsertMany(uniqueDocs),
                out var error
            );

            if (!isSuccessful)
            {
                writer.WriteLine($"Failed to add all new {collectionName} documents");
                writer.WriteLine(error.WriteErrorJsonString);
            }
        }
        
        /// <summary>
        /// Returns all sprites
        /// </summary>
        public static IEnumerable<SpriteModelv1> GetAllSprites()
        {
            return MongoCollectionHelper.Sprite.Find(sprite => true).ToEnumerable();
        }

        /// <summary>
        /// Attempts to add a sprite using the provided document
        /// </summary>
        /// <param name="sprites">The documents to add</param>
        /// <param name="error">Any error found</param>
        public static bool TryAddSprite(
            ICollection<SpriteModelv1> sprites,
            out MongoWriteError error)
        {
            try
            {
                MongoCollectionHelper.Sprite.InsertMany(sprites);
                error = null;
                return true;
            }
            catch (MongoWriteException exception)
            {
                error = new MongoWriteError(exception.WriteError.Details.GetValue("details").AsBsonDocument.ToString());
                return false;
            }
        }

        private static bool TryAddDexEntry(
            Action action,
            out MongoWriteError error)
        {
            try
            {
                action();
                error = null;
                return true;
            }
            catch (MongoWriteException exception)
            {
                error = new MongoWriteError(exception.WriteError.Details.GetValue("details").AsBsonDocument.ToString());
                return false;
            }
        }
    }
}
