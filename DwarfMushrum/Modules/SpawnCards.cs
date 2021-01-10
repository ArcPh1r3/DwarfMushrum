using R2API;
using RoR2;
using RoR2.Navigation;
using System.Collections.Generic;
using UnityEngine;

namespace DwarfMushrum.Modules
{
    public static class SpawnCards
    {
        public static CharacterSpawnCard monsterSpawnCard;

        public static void CreateSpawnCards()
        {
            monsterSpawnCard = ScriptableObject.CreateInstance<CharacterSpawnCard>();
            monsterSpawnCard.name = "cscDwarfMushrum";
            monsterSpawnCard.prefab = Prefabs.masterPrefab;
            monsterSpawnCard.sendOverNetwork = true;
            monsterSpawnCard.hullSize = HullClassification.Human;
            monsterSpawnCard.nodeGraphType = MapNodeGroup.GraphType.Ground;
            monsterSpawnCard.requiredFlags = NodeFlags.None;
            monsterSpawnCard.forbiddenFlags = NodeFlags.None;
            monsterSpawnCard.directorCreditCost = 27;
            monsterSpawnCard.occupyPosition = true;
            monsterSpawnCard.loadout = new SerializableLoadout();
            monsterSpawnCard.noElites = false;
            monsterSpawnCard.forbiddenAsBoss = true;
        }

        public static void RegisterSpawnCards()
        {
            DirectorCard card = new DirectorCard
            {
                spawnCard = monsterSpawnCard,
                selectionWeight = 1,
                allowAmbushSpawn = false,
                preventOverhead = false,
                minimumStageCompletions = 0,
                requiredUnlockable = "",
                forbiddenUnlockable = "",
                spawnDistance = DirectorCore.MonsterSpawnDistance.Far
            };

            DirectorAPI.DirectorCardHolder monsterCard = new DirectorAPI.DirectorCardHolder
            {
                Card = card,
                MonsterCategory = DirectorAPI.MonsterCategory.BasicMonsters,
                InteractableCategory = DirectorAPI.InteractableCategory.None
            };

            DirectorAPI.MonsterActions += delegate (List<DirectorAPI.DirectorCardHolder> list, DirectorAPI.StageInfo stage)
            {
                if (stage.stage == DirectorAPI.Stage.SkyMeadow || stage.stage == DirectorAPI.Stage.DistantRoost || stage.stage == DirectorAPI.Stage.WetlandAspect || stage.stage == DirectorAPI.Stage.VoidCell)
                {
                    if (!list.Contains(monsterCard))
                    {
                        list.Add(monsterCard);
                    }
                }

                if (stage.stage == DirectorAPI.Stage.Custom && stage.CustomStageName == "rootjungle")
                {
                    if (!list.Contains(monsterCard))
                    {
                        list.Add(monsterCard);
                    }
                }
            };
        }
    }
}
