﻿using BepInEx.Configuration;
using System.Collections.Generic;
using System.Linq;

namespace DwarfMushrum.Modules
{
    internal static class Config
    {
        public static List<StageSpawnInfo> StageList = new List<StageSpawnInfo>();

        internal static void ReadConfig()
        {
            string stages = DwarfMushrumPlugin.instance.Config.Bind<string>(
                "Dwarf Mushrum",
                "Stage List",
                "arena, blackbeach, blackbeach2, foggyswamp, itgoolake, itskymeadow, rootjungle, shipgraveyard, skymeadow, sulfurpools, goldshores",
                "What stages the monster will show up on. Add a '- loop' after the stagename to make it only spawn after looping. List of stage names can be found at https://github.com/risk-of-thunder/R2Wiki/wiki/Mod-Creation_Developer-Reference_Scene-Names").Value;

            //parse stage
            stages = new string(stages.ToCharArray().Where(c => !System.Char.IsWhiteSpace(c)).ToArray());
            string[] splitStages = stages.Split(',');
            foreach (string str in splitStages)
            {
                string[] current = str.Split('-');

                string name = current[0];
                int minStages = 0;
                if (current.Length > 1)
                {
                    minStages = 5;
                }

                StageList.Add(new StageSpawnInfo(name, minStages));
            }
        }

        internal static ConfigEntry<bool> EnemyEnableConfig(string characterName)
        {
            return DwarfMushrumPlugin.instance.Config.Bind<bool>(new ConfigDefinition(characterName, "Enabled"), true, new ConfigDescription("Set to false to disable this enemy"));
        }

        internal static ConfigEntry<bool> CharacterEnableConfig(string characterName)
        {
            return DwarfMushrumPlugin.instance.Config.Bind<bool>(new ConfigDefinition(characterName, "Enabled"), false, new ConfigDescription("Set to false to disable this character"));
        }
    }


    public class StageSpawnInfo
    {
        private string stageName;
        private int minStages;

        public StageSpawnInfo(string stageName, int minStages)
        {
            this.stageName = stageName;
            this.minStages = minStages;
        }

        public string GetStageName() { return stageName; }
        public int GetMinStages() { return minStages; }
    }
}