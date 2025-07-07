using MongoDB.Bson;
using MongoDbImportTool.Interfaces;

namespace MongoDbImportTool.Models
{
    /// <summary>
    /// Represents an feature in the FeatureDex
    /// </summary>
    public class FeatureModelv1 : IDocument, IDexDocument
    {
        /// <inheritdoc/>
        public ObjectId _id { get; set; }

        /// <inheritdoc/>
        public string Name { get; set; }

        /// <summary>
        /// The effects of using the feature
        /// </summary>
        public string Effects { get; set; }
    }
}
