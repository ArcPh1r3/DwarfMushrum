using System;
using EntityStates;
using RoR2;
using RoR2.Projectile;
using UnityEngine;

namespace DwarfMushrum.States
{
    public class FireProjectile : BaseSkillState
    {
        public static float baseDuration = 0.1f;
        public static float damageCoefficient = 1.2f;
        public static float force = 10f;

        private float duration;

        public override void OnEnter()
        {
            base.OnEnter();
            this.duration = FireProjectile.baseDuration / this.attackSpeedStat;

            Util.PlaySound(EntityStates.MiniMushroom.SporeGrenade.attackSoundString, base.gameObject);

            Ray aimRay = base.GetAimRay();
            string muzzleName = "Muzzle";

            if (EntityStates.Mage.Weapon.FireLaserbolt.muzzleEffectPrefab)
            {
                EffectManager.SimpleMuzzleFlash(EntityStates.Mage.Weapon.FireLaserbolt.muzzleEffectPrefab, base.gameObject, muzzleName, false);
            }

            if (base.isAuthority)
            {
                ProjectileManager.instance.FireProjectile(EntityStates.MiniMushroom.SporeGrenade.projectilePrefab, aimRay.origin, Util.QuaternionSafeLookRotation(aimRay.direction), base.gameObject, this.damageStat * FireProjectile.damageCoefficient, FireProjectile.force, base.RollCrit(), DamageColorIndex.Default, null, -1f);
            }
        }

        public override void FixedUpdate()
        {
            base.FixedUpdate();

            if (base.fixedAge >= this.duration && base.isAuthority)
            {
                this.outer.SetNextStateToMain();
                return;
            }
        }

        public override InterruptPriority GetMinimumInterruptPriority()
        {
            return InterruptPriority.Skill;
        }
    }
}