using EntityStates;
using R2API;
using RoR2;
using System;
using System.Collections.Generic;
using UnityEngine;

namespace DwarfMushrum.Modules
{
    public class States
    {
        internal static List<Type> entityStates = new List<Type>();

        internal static void AddSkill(Type t)
        {
            entityStates.Add(t);
        }

        public static void RegisterStates()
        {
            GameObject body = Prefabs.bodyPrefab;

            AddSkill(typeof(DwarfMushrum.States.ChargeProjectile));
            AddSkill(typeof(DwarfMushrum.States.FireProjectile));
            AddSkill(typeof(DwarfMushrum.States.Burrow));

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
