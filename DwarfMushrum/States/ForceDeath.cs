using EntityStates;
using RoR2;

namespace DwarfMushrum.States
{
    public class ForceDeath : BaseSkillState
    {
        public override void OnEnter()
        {
            base.OnEnter();

            if (base.isAuthority) base.healthComponent.Suicide(null, null, DamageType.Generic);

            base.outer.SetNextStateToMain();
        }
    }
}
