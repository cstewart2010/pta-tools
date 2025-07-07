﻿using MongoDB.Driver;
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
        /// Attempts to evolve a pokemon to its next stage
        /// </summary>
        /// <param name="pokemon">The current form</param>
        /// <param name="keptMoves">The moves you wish to keep</param>
        /// <param name="evolvedName">The name of the evolved form</param>
        /// <param name="newMoves">The moves you wish to add</param>
        public static PokemonModelv1 GetEvolved(
            PokemonModelv1 pokemon,
            IEnumerable<string> keptMoves,
            string evolvedName,
            IEnumerable<string> newMoves)
        {
            ArgumentNullException.ThrowIfNull(pokemon, nameof(pokemon));
            ArgumentNullException.ThrowIfNull(keptMoves, nameof(keptMoves));
            if (string.IsNullOrEmpty(evolvedName)) throw ExceptionHandler.IsNullOrEmpty(nameof(evolvedName));

            var (basePokemon, altForms) = GetPokedexEntry(evolvedName, pokemon.Form);
            if (!string.Equals(basePokemon?.EvolvesFrom, pokemon.SpeciesName, StringComparison.CurrentCultureIgnoreCase))
            {
                return null;
            }

            var evolvedMoves = basePokemon.Moves.Select(move => move.ToLower());
            if (!newMoves.All(move => evolvedMoves.Contains(move.ToLower())))
            {
                return null;
            }

            return new PokemonModelv1
            {
                PokemonId = pokemon.PokemonId,
                DexNo = basePokemon.DexNo,
                SpeciesName = basePokemon.Name,
                Nickname = pokemon.Nickname,
                Gender = pokemon.Gender,
                PokemonStatus = pokemon.PokemonStatus,
                Moves = keptMoves.Union(newMoves),
                Type = basePokemon.Type,
                CatchRate = GetCatchRate(basePokemon),
                Nature = pokemon.Nature,
                IsShiny = pokemon.IsShiny,
                PokemonStats = basePokemon.PokemonStats,
                Size = basePokemon.Size,
                Weight = basePokemon.Weight,
                Skills = basePokemon.Skills,
                Passives = basePokemon.Passives,
                Proficiencies = basePokemon.Proficiencies,
                EggGroups = basePokemon.EggGroups,
                EggHatchRate = basePokemon.EggHatchRate,
                Habitats = basePokemon.Habitats,
                Diet = basePokemon.Diet,
                Rarity = basePokemon.Rarity,
                GMaxMove = basePokemon.GMaxMove,
                EvolvedFrom = basePokemon.EvolvesFrom,
                LegendaryStats = basePokemon.LegendaryStats,
                IsOnActiveTeam = pokemon.IsOnActiveTeam,
                AlternateForms = altForms,
                NormalPortrait = basePokemon.NormalPortrait,
                ShinyPortrait = basePokemon.ShinyPortrait,
                TrainerId = pokemon.TrainerId,
                OriginalTrainerId = pokemon.OriginalTrainerId,
                Form = pokemon.Form
            };
        }

        /// <summary>
        /// Returns a collection of possible evolutions
        /// </summary>
        /// <param name="pokemon"></param>
        public static IEnumerable<BasePokemonModelv1> GetPossibleEvolutions(PokemonModelv1 pokemon)
        {
            var collection = MongoCollectionHelper.Database.GetCollection<BasePokemonModelv1>(DexType.BasePokemon.ToString());
            var allEvolutions = collection.Find(document => document.EvolvesFrom.Equals(pokemon.SpeciesName, StringComparison.CurrentCultureIgnoreCase)).ToEnumerable();
            return allEvolutions.Where(evolution => evolution.Form.Equals(pokemon.Form, StringComparison.CurrentCultureIgnoreCase));
        }

        /// <summary>
        /// Builds a <see cref="PokemonModelv1"/> using information from the <see cref="BasePokemonModelv1"/>
        /// </summary>
        /// <param name="name">The pokemon's species name</param>
        /// <param name="nickname">The pokemon's nickname, if applicable</param>
        /// <param name="form">The pokemon's form</param>
        public static PokemonModelv1 GetNewPokemon(string name, string nickname, string form)
        {
            var random = new Random();
            var nature = (Nature)random.Next(1, 21);
            var gender = (Gender)random.Next(3);
            var status = Status.Normal;
            return GetNewPokemon(name, nature, gender, status, nickname, form);
        }

        /// <summary>
        /// Builds a <see cref="PokemonModelv1"/> using information from the <see cref="BasePokemonModelv1"/>
        /// </summary>
        /// <param name="name">The pokemon's species name</param>
        /// <param name="nature">The nature to give the pokemon</param>
        /// <param name="gender">The pokemon's gender</param>
        /// <param name="status">The pokemon's status</param>
        /// <param name="nickname">The pokemon's nickname, if applicable</param>
        /// <param name="form">The pokemon's form</param>
        public static PokemonModelv1 GetNewPokemon(
            string name,
            Nature nature,
            Gender gender,
            Status status,
            string nickname,
            string form)
        {
            var (basePokemon, altForms) = GetPokedexEntry(name, form);
            return GetPokemonFromBase(basePokemon, nature, gender, status, nickname, altForms);
        }

        /// <summary>
        /// Returns all Dex extries for a specific Dex collection
        /// </summary>
        /// <param name="documentType">The dex collection you wish to return data from</param>
        public static IEnumerable<TDocument> GetDexEntries<TDocument>(DexType documentType) where TDocument : IDexDocument
        {
            var collection = MongoCollectionHelper.Database.GetCollection<TDocument>(documentType.ToString());
            return collection.Find(document => true).ToEnumerable();
        }

        /// <summary>
        /// Returns a specific Dex entry from a specific Dex collection
        /// </summary>
        /// <param name="name">The name of the dex entry</param>
        /// <param name="form">The pokemon form to select</param>
        public static (BasePokemonModelv1 Model, IEnumerable<string> AlternateForms) GetPokedexEntry(
            string name,
            string form)
        {
            var collection = MongoCollectionHelper.Database.GetCollection<BasePokemonModelv1>(DexType.BasePokemon.ToString());
            var allForms = collection.Find(document => document.Name.Equals(name, StringComparison.CurrentCultureIgnoreCase)).ToEnumerable();
            var model = allForms.First(document => document.Form.Equals(form, StringComparison.CurrentCultureIgnoreCase));
            var alternateForms = allForms.Where(document => !document.Form.Equals(form, StringComparison.CurrentCultureIgnoreCase))
                .Select(document => document.Form);
            return (model, alternateForms);
        }

        /// <summary>
        /// Returns an origin model from the origin collection
        /// </summary>
        /// <param name="Origin">The name of the origin</param>
        public static OriginModelv1 GetOrigin(String Origin) 
        { 
            var collection = MongoCollectionHelper.Database.GetCollection<OriginModelv1>(DexType.Origins.ToString());
            var model = collection.Find(document => document.Name.Equals(Origin, StringComparison.OrdinalIgnoreCase)).SingleOrDefault();
                return model;
        }

        /// <summary>
        /// Returns a specific Dex entry from a specific Dex collection
        /// </summary>
        /// <param name="documentType">The dex collection you wish to return data from</param>
        /// <param name="name">The name of the dex entry</param>
        public static TDocument GetDexEntry<TDocument>(
            DexType documentType,
            string name) where TDocument : IDexDocument
        {
            var collection = MongoCollectionHelper.Database.GetCollection<TDocument>(documentType.ToString());
            return collection.Find(document => document.Name.Equals(name, StringComparison.CurrentCultureIgnoreCase)).FirstOrDefault();
        }

        /// <summary>
        /// Adds a collection of dex entry to a specific dex collection
        /// </summary>
        /// <param name="documents">The documents to add to collection</param>
        public static void AddPokedexEntries(IEnumerable<BasePokemonModelv1> documents)
        {
            var collection = MongoCollectionHelper.Database.GetCollection<BasePokemonModelv1>(DexType.BasePokemon.ToString());
            foreach (var document in documents)
            {
                if (collection.Find(currentDocument => document.Name == currentDocument.Name && document.Form == currentDocument.Form).Any())
                {
                    continue;
                }

                Console.WriteLine($"Adding {document.Name} to {DexType.BasePokemon}");
                var isSuccessful = TryAddDexEntry
                (
                    () => collection.InsertOne(document),
                    out var error
                );

                if (!isSuccessful)
                {
                    Console.WriteLine($"Failed to add {document.Name} to {DexType.BasePokemon}");
                    Console.WriteLine(error.WriteErrorJsonString);
                }
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
            foreach (var document in documents)
            {
                if (collection.Find(currentDocument => document.Name == currentDocument.Name).Any())
                {
                    continue;
                }

                writer.WriteLine($"Adding {document.Name} to {collectionName}");
                var isSuccessful = TryAddDexEntry
                (
                    () => collection.InsertOne(document),
                    out var error
                );

                if (!isSuccessful)
                {
                    writer.WriteLine($"Failed to add {document.Name} to {collectionName}");
                    writer.WriteLine(error.WriteErrorJsonString);
                }
            }
        }

        private static PokemonModelv1 GetPokemonFromBase(
            BasePokemonModelv1 basePokemon,
            Nature nature,
            Gender gender,
            Status status,
            string nickname,
            IEnumerable<string> altForms)
        {
            if (basePokemon == null)
            {
                return null;
            }

            var updatedNickname = string.IsNullOrWhiteSpace(nickname)
                ? basePokemon.Name
                : nickname;

            var stats = new StatsModelv1
            {
                HP = basePokemon.PokemonStats.HP,
                Attack = basePokemon.PokemonStats.Attack,
                Defense = basePokemon.PokemonStats.Defense,
                SpecialAttack = basePokemon.PokemonStats.SpecialAttack,
                SpecialDefense = basePokemon.PokemonStats.SpecialDefense,
                Speed = basePokemon.PokemonStats.Speed,
            };

            return new PokemonModelv1
            {
                PokemonId = Guid.NewGuid(),
                DexNo = basePokemon.DexNo,
                SpeciesName = basePokemon.Name,
                Nickname = updatedNickname,
                Gender = gender.ToString(),
                PokemonStatus = status.ToString(),
                Moves = basePokemon.Moves,
                Type = basePokemon.Type,
                CatchRate = GetCatchRate(basePokemon),
                Nature = nature.ToString(),
                IsShiny = new Random().Next(420) == 69,
                PokemonStats = stats,
                CurrentHP = stats.HP,
                Size = basePokemon.Size,
                Weight = basePokemon.Weight,
                Skills = basePokemon.Skills,
                Passives = basePokemon.Passives,
                Proficiencies = basePokemon.Proficiencies,
                EggGroups = basePokemon.EggGroups,
                EggHatchRate = basePokemon.EggHatchRate,
                Habitats = basePokemon.Habitats,
                Diet = basePokemon.Diet,
                Rarity = basePokemon.Rarity,
                GMaxMove = basePokemon.GMaxMove,
                EvolvedFrom = basePokemon.EvolvesFrom,
                LegendaryStats = basePokemon.LegendaryStats,
                Form = basePokemon.Form,
                AlternateForms = altForms,
                NormalPortrait = basePokemon.NormalPortrait,
                ShinyPortrait = basePokemon.ShinyPortrait
            };
        }

        private static int GetCatchRate(BasePokemonModelv1 basePokemon)
        {
            Enum.TryParse(basePokemon.Rarity, true, out Rarity rarity);
            return rarity switch
            {
                Rarity.Common => 50,
                Rarity.Uncommon => 40,
                _ => 30,
            } - (15 * (basePokemon.Stage - 1));
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
