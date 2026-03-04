using MongoDB.Bson;

namespace MongoDbImportTool.Interfaces
{
    /// <summary>
    /// Provides a collection of properties for MongoDB documents
    /// </summary>
    public interface IDexDocument
    {
        /// <summary>
        /// MongoDB id
        /// </summary>
        public ObjectId _id { get; set; }
        
        /// <summary>
        /// The name of the dex document
        /// </summary>
        public string Name { get; set; }
    }
}
