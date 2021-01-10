using EntityStates;
using R2API;
using RoR2;
using UnityEngine;

namespace DwarfMushrum.Modules
{
    public class States
    {
        public static void RegisterStates()
        {
            GameObject body = Prefabs.bodyPrefab;

            LoadoutAPI.AddSkill(typeof(DwarfMushrum.States.ChargeProjectile));
            LoadoutAPI.AddSkill(typeof(DwarfMushrum.States.FireProjectile));
            LoadoutAPI.AddSkill(typeof(DwarfMushrum.States.Burrow));

            var stateMachine = body.GetComponentInChildren<EntityStateMachine>();
            if (stateMachine)
            {
                stateMachine.initialStateType = new SerializableEntityStateType(typeof(EntityStates.MiniMushroom.SpawnState));
            }

            var deathBehavior = body.GetComponentInChildren<CharacterDeathBehavior>();
            if (deathBehavior)
            {
                deathBehavior.deathState = new SerializableEntityStateType(typeof(EntityStates.GenericCharacterDeath));
            }
        }
    }
}
