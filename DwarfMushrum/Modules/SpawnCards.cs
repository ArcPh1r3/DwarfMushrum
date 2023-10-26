using BepInEx.Configuration;
using R2API;
using RoR2;
using RoR2.Navigation;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AddressableAssets;

namespace DwarfMushrum.Modules
{
    public static class SpawnCards
    {
        public static CharacterSpawnCard monsterSpawnCard;

        internal static ConfigEntry<int> minimumStageCount;
        internal static ConfigEntry<int> spawnCost;

        public static void CreateSpawnCards()
        {
            minimumStageCount = DwarfMushrumPlugin.instance.Config.Bind<int>(new ConfigDefinition("Dwarf Mushrum", "Minimum Stage Clear Count"), 0, new ConfigDescription("Number of stages that must be completed before this monster can spawn"));
            spawnCost = DwarfMushrumPlugin.instance.Config.Bind<int>(new ConfigDefinition("Dwarf Mushrum", "Spawn Cost"), 27, new ConfigDescription("How many director credits does this monster cost"));

            CharacterSpawnCard characterSpawnCard = ScriptableObject.CreateInstance<CharacterSpawnCard>();
            characterSpawnCard.name = "cscDwarfMushrum";
            characterSpawnCard.prefab = Prefabs.masterPrefab;
            characterSpawnCard.sendOverNetwork = true;
            characterSpawnCard.hullSize = HullClassification.Human;
            characterSpawnCard.nodeGraphType = MapNodeGroup.GraphType.Ground;
            characterSpawnCard.requiredFlags = NodeFlags.None;
            characterSpawnCard.forbiddenFlags = NodeFlags.TeleporterOK;
            characterSpawnCard.directorCreditCost = spawnCost.Value;
            characterSpawnCard.occupyPosition = false;
            characterSpawnCard.loadout = new SerializableLoadout();
            characterSpawnCard.noElites = false;
            characterSpawnCard.forbiddenAsBoss = false;

            DirectorCard card = new DirectorCard
            {
                spawnCard = characterSpawnCard,
                selectionWeight = 1,
                preventOverhead = false,
                minimumStageCompletions = minimumStageCount.Value,
                spawnDistance = DirectorCore.MonsterSpawnDistance.Standard
            };

            DirectorAPI.DirectorCardHolder spawnCard = new DirectorAPI.DirectorCardHolder
            {
                Card = card,
                MonsterCategory = DirectorAPI.MonsterCategory.BasicMonsters,
            };

            DirectorCard cardLoop = new DirectorCard
            {
                spawnCard = characterSpawnCard,
                selectionWeight = 1,
                preventOverhead = false,
                minimumStageCompletions = 5,
                spawnDistance = DirectorCore.MonsterSpawnDistance.Far
            };

            DirectorAPI.DirectorCardHolder spawnCardLoop = new DirectorAPI.DirectorCardHolder
            {
                Card = cardLoop,
                MonsterCategory = DirectorAPI.MonsterCategory.BasicMonsters,
            };

            DirectorCardCategorySelection dissonanceSpawns = Addressables.LoadAssetAsync<DirectorCardCategorySelection>("RoR2/Base/MixEnemy/dccsMixEnemy.asset").WaitForCompletion();
            dissonanceSpawns.AddCard(1, card);  //0 is Champions

            foreach (StageSpawnInfo ssi in Config.StageList)
            {
                DirectorAPI.DirectorCardHolder toAdd = ssi.GetMinStages() == 0 ? spawnCard : spawnCardLoop;

                DirectorAPI.Helpers.AddNewMonsterToStage(toAdd, false, DirectorAPI.ParseInternalStageName(ssi.GetStageName()), ssi.GetStageName());
            }
        }
    }
}
