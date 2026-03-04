using System.Collections.Generic;
using MongoDbImportTool.Enums;
using MongoDbImportTool.Models;

namespace MongoDbImportTool.Builders
{
    internal static class FeatureBuilder
    {
        private static readonly string FeaturesJson = $"{JsonHelper.CurrentDirectory}/json/features.min.json";
        private static readonly string LegendaryFeaturesJson = $"{JsonHelper.CurrentDirectory}/json/legendary_features.min.json";
        private static readonly string SkillsJson = $"{JsonHelper.CurrentDirectory}/json/skills.min.json";
        private static readonly string PassivesJson = $"{JsonHelper.CurrentDirectory}/json/passives.min.json";

        public static void AddFeatures()
        {
            TaskHelper.RunAsyncTasks
            (
                () => DatabaseHelper.AddDocuments(nameof(DexType.Features), GetFeatures(FeaturesJson)),
                () => DatabaseHelper.AddDocuments(nameof(DexType.LegendaryFeatures), GetFeatures(LegendaryFeaturesJson)),
                () => DatabaseHelper.AddDocuments(nameof(DexType.Skills), GetFeatures(SkillsJson)),
                () => DatabaseHelper.AddDocuments(nameof(DexType.Passives), GetFeatures(PassivesJson))
            );
        }

        private static IEnumerable<FeatureModelv1> GetFeatures(string path)
        {
            foreach (var child in JsonHelper.GetToken(path))
            {
                yield return JsonHelper.BuildFeature(child);
            }
        }
    }
}
