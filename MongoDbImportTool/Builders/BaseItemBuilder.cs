using System.Collections.Generic;
using MongoDbImportTool.Enums;
using MongoDbImportTool.Models;

namespace MongoDbImportTool.Builders
{
    internal static class BaseItemBuilder
    {
        private static readonly string KeyItemsJson = $"{JsonHelper.CurrentDirectory}/json/key_items.min.json";
        private static readonly string MedicalItemsJson = $"{JsonHelper.CurrentDirectory}/json/medical_items.min.json";
        private static readonly string PokeballsJson = $"{JsonHelper.CurrentDirectory}/json/pokeballs.min.json";
        private static readonly string PokemonItemsJson = $"{JsonHelper.CurrentDirectory}/json/pokemon_items.min.json";
        private static readonly string TrainerEquipmentJson = $"{JsonHelper.CurrentDirectory}/json/trainer_equipment.min.json";

        public static void AddItems()
        {
            TaskHelper.RunAsyncTasks
            (
                () => DatabaseHelper.AddDocuments(nameof(DexType.KeyItems), GetItems(KeyItemsJson)),
                () => DatabaseHelper.AddDocuments(nameof(DexType.MedicalItems), GetItems(MedicalItemsJson)),
                () => DatabaseHelper.AddDocuments(nameof(DexType.Pokeballs), GetItems(PokeballsJson)),
                () => DatabaseHelper.AddDocuments(nameof(DexType.PokemonItems), GetItems(PokemonItemsJson)),
                () => DatabaseHelper.AddDocuments(nameof(DexType.TrainerEquipment), GetItems(TrainerEquipmentJson))
            );
        }

        private static IEnumerable<BaseItemModelV1> GetItems(string path)
        {
            foreach (var child in JsonHelper.GetToken(path))
            {
                yield return JsonHelper.BuildItem(child);
            }
        }
    }
}
