using System;
using System.Collections.Generic;
using MongoDbImportTool.Interfaces;
using MongoDbImportTool.Utilities;

namespace MongoDbImportTool
{
    internal static class DatabaseHelper
    {
        public static void AddDocuments<TDocument>(
            string collectionName,
            IEnumerable<TDocument> documents) where TDocument : IDexDocument
        {
            DexUtility.AddDexEntries(collectionName, documents, Console.Out);
        }
    }
}
