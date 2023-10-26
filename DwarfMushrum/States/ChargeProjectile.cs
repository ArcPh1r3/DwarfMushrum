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

            this.playID = Util.PlayAttackSpeedSound(EntityStates.MiniMushroom.SporeGrenade.chargeUpSoundString, base.gameObject, this.attackSpeedStat);

            if (modelTransform)
            {
                Transform transform = base.FindModelChild("Muzzle");
                this.chargeVfxInstance = UnityEngine.Object.Instantiate<GameObject>(EntityStates.MiniMushroom.SporeGrenade.chargeEffectPrefab, transform.position, transform.rotation);
                this.chargeVfxInstance.transform.parent = transform;
                this.chargeVfxInstance.transform.localPosition = new Vector3(0f, -0.065f, 0f);
                this.chargeVfxInstance.transform.localRotation = Quaternion.identity;
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
