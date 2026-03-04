using System;
using System.Collections.Generic;
using System.Linq;
using MongoDbImportTool.Models;
using MongoDbImportTool.Utilities;

namespace MongoDbImportTool.Builders
{
    internal static class SpriteBuilder
    {
        private static readonly string SpritesJson = $"{JsonHelper.CurrentDirectory}/json/sprites.min.json";

        public static void AddSprites()
        {
            var collection = DexUtility.GetAllSprites();
            var documents = GetSprites(SpritesJson).Where(x => collection.All(y => x.Value != y.Value)).ToList();
            if (documents.Count == 0)
            {
                Console.WriteLine($"No new Sprites to add");
                return;
            }
            Console.WriteLine($"Adding new Sprites");
            var isSuccessful = DexUtility.TryAddSprite(documents, out var error);

            if (!isSuccessful)
            {
                Console.WriteLine($"Failed to add all to Sprites");
                Console.WriteLine(error.WriteErrorJsonString);
            }
        }

        private static IEnumerable<SpriteModelv1> GetSprites(string path)
        {
            foreach (var child in JsonHelper.GetToken(path))
            {
                yield return child.ToObject<SpriteModelv1>();
            }
        }
    }
}
