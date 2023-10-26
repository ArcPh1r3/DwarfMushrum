using System;
using BepInEx;
using R2API.Utils;
using System.Security;
using System.Security.Permissions;
using UnityEngine;

[module: UnverifiableCode]
[assembly: SecurityPermission(SecurityAction.RequestMinimum, SkipVerification = true)]

namespace DwarfMushrum
{
    [BepInDependency("com.bepis.r2api", BepInDependency.DependencyFlags.HardDependency)]
    [NetworkCompatibility(CompatibilityLevel.EveryoneMustHaveMod, VersionStrictness.EveryoneNeedSameModVersion)]
    [BepInPlugin(MODUID, "DwarfMushrum", "1.1.0")]

    public class DwarfMushrumPlugin : BaseUnityPlugin
    {
        public const string MODUID = "com.rob.DwarfMushrum";

        public static DwarfMushrumPlugin instance;

        public static Material commandoMat; // cloning this to create materials

        private void Awake()
        {
            instance = this;

            Modules.Assets.PopulateAssets();// loading in the assetbundle and caching assets for easy reference
            Modules.Config.ReadConfig(); // config
            Modules.Tokens.RegisterLanguageTokens(); // add language tokens for all added text

            Modules.Prefabs.CreatePrefab(); // create the necessary prefabs, both body and master(ai) are created in here
            Modules.States.RegisterStates(); // register all custom entitystates, won't work in mp otherwise
            Modules.Skills.RegisterSkills(); // set up skills for the body prefab
            Modules.Projectiles.CreateProjectiles(); // create and register all the custom projectiles to be used
            Modules.SpawnCards.CreateSpawnCards(); // create the spawn card needed to spawn the monster and add it to the pool

            new Modules.ContentPacks().Initialize(); // initialize the content pack and actually add your stuff to the game
        }
    }
}