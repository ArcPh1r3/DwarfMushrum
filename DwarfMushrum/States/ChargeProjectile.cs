using EntityStates;
using RoR2;
using UnityEngine;

namespace DwarfMushrum.States
{
    public class ChargeProjectile : BaseSkillState
    {
        public static float baseDuration = 1.75f;

        private float duration;
        private GameObject chargeVfxInstance;
        private uint playID;

        public override void OnEnter()
        {
            base.OnEnter();
            base.StartAimMode(2f, false);
            this.duration = ChargeProjectile.baseDuration / this.attackSpeedStat;
            Transform modelTransform = base.GetModelTransform();

            this.playID = Util.PlayScaledSound(EntityStates.MiniMushroom.SporeGrenade.chargeUpSoundString, base.gameObject, this.attackSpeedStat);

            if (modelTransform)
            {
                Transform transform = base.FindModelChild("Muzzle");
                this.chargeVfxInstance = UnityEngine.Object.Instantiate<GameObject>(EntityStates.MiniMushroom.SporeGrenade.chargeEffectPrefab, transform.position, transform.rotation);
                this.chargeVfxInstance.transform.parent = transform;
            }

            base.PlayAnimation("FullBody, Override", "ShootSpores", "ShootSpores.playbackRate", this.duration * 2.5f);
        }

        public override void OnExit()
        {
            base.OnExit();

            if (this.chargeVfxInstance)
            {
                EntityState.Destroy(this.chargeVfxInstance);
            }

            AkSoundEngine.StopPlayingID(this.playID);
        }

        public override void FixedUpdate()
        {
            base.FixedUpdate();

            if (base.fixedAge >= this.duration && base.isAuthority)
            {
                FireProjectile nextState = new FireProjectile();
                this.outer.SetNextState(nextState);
                return;
            }
        }

        public override InterruptPriority GetMinimumInterruptPriority()
        {
            return InterruptPriority.Skill;
        }
    }
}
