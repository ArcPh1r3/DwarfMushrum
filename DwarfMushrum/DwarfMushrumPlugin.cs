using System;
using BepInEx;
using R2API.Utils;
using UnityEngine;

namespace DwarfMushrum
{
    [BepInDependency("com.bepis.r2api", BepInDependency.DependencyFlags.HardDependency)]
    [NetworkCompatibility(CompatibilityLevel.EveryoneMustHaveMod, VersionStrictness.EveryoneNeedSameModVersion)]
    [BepInPlugin(MODUID, "DwarfMushrum", "1.0.0")]
    [R2APISubmoduleDependency(new string[]
    {
        "PrefabAPI",
        "LoadoutAPI",
        "LanguageAPI",
        "SoundAPI",
        "EffectAPI",
        "ResourcesAPI"
    })]

    public class DwarfMushrumPlugin : BaseUnityPlugin
    {
        public const string MODUID = "com.rob.DwarfMushrum";

        public static DwarfMushrumPlugin instance;

        public static Material commandoMat; // cloning this to create materials

        public static event Action awake;

        public DwarfMushrumPlugin()
        {
            awake += DwarfMushrumPlugin_Load;
        }

        private void DwarfMushrumPlugin_Load()
        {
            instance = this;

            Modules.Assets.PopulateAssets();// loading in the assetbundle and caching assets for easy reference
            //Modules.Config.ReadConfig(); // config if it were implemented
            Modules.Tokens.RegisterLanguageTokens(); // add language tokens for all added text

            Modules.Prefabs.CreatePrefab(); // create the necessary prefabs, both body and master(ai) are created in here
            Modules.States.RegisterStates(); // register all custom entitystates, won't work in mp otherwise
            Modules.Skills.RegisterSkills(); // set up skills for the body prefab
            Modules.Projectiles.CreateProjectiles(); // create and register all the custom projectiles to be used
            Modules.SpawnCards.CreateSpawnCards(); // create the spawn card needed to spawn the monster
            Modules.SpawnCards.RegisterSpawnCards(); // add the card to spawn pool
        }

        // plugin constructor, nothing to see here
        public void Awake()
        {
            Action awake = DwarfMushrumPlugin.awake;
            if (awake == null)
            {
                return;
            }
            awake();
        }
    }
}