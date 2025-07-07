﻿using System;
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
            var collection = DatabaseUtility.GetAllSprites();
            foreach (var document in GetSprites(SpritesJson))
            {
                if (collection.Any(currentDocument => document.Value == currentDocument.Value))
                {
                    continue;
                }

                Console.WriteLine($"Adding {document.FriendlyText} to Sprites");
                var isSuccessful = DatabaseUtility.TryAddSprite(document, out var error);

                if (!isSuccessful)
                {
                    Console.WriteLine($"Failed to add {document.FriendlyText} to Sprites");
                    Console.WriteLine(error.WriteErrorJsonString);
                }
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
