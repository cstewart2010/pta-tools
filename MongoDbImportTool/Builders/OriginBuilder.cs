﻿using Newtonsoft.Json.Linq;
using System.Collections.Generic;
using MongoDbImportTool.Models;

namespace MongoDbImportTool.Builders
{
    internal static class OriginBuilder
    {
        private static readonly string OriginsJson = $"{JsonHelper.CurrentDirectory}/json/origins.min.json";
        private static readonly string OriginEquipmentJson = $"{JsonHelper.CurrentDirectory}/json/origin_equipment.min.json";
        private static JToken _equipmentToken;

        public static void AddOrigins()
        {
            _equipmentToken = JsonHelper.GetToken(OriginEquipmentJson);
            DatabaseHelper.AddDocuments("Origins", GetOrigins(OriginsJson));
        }

        private static IEnumerable<OriginModelv1> GetOrigins(string path)
        {
            foreach (var child in JsonHelper.GetToken(path))
            {
                yield return Build(child);
            }
        }

        private static OriginModelv1 Build(JToken originToken)
        {
            var model =  new OriginModelv1
            {
                Name = JsonHelper.GetNameFromToken(originToken),
                Skill = JsonHelper.GetStringFromToken(originToken, "Skill Talent"),
                Lifestyle = JsonHelper.GetStringFromToken(originToken, "Lifestyle"),
                Savings = JsonHelper.GetIntFromToken(originToken, "Savings"),
                Equipment = JsonHelper.GetStringFromToken(originToken, "Starting Equipment"),
                StartingPokemon = JsonHelper.GetStringFromToken(originToken, "Starting Pokemon"),
                Feature = new FeatureModelv1
                {
                    Name = JsonHelper.GetStringFromToken(originToken, "Feature Name"),
                    Effects = JsonHelper.GetStringFromToken(originToken, "Feature Effects")
                }                
            };

            model.StartingEquipmentList = _equipmentToken[model.Name].ToObject<IEnumerable<StartingEquipment>>();
            return model;
        }
    }
}
