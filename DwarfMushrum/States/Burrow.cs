using System.Linq;
using EntityStates;
using RoR2;
using RoR2.Navigation;
using UnityEngine;

namespace DwarfMushrum.States
{
    public class Burrow : BaseState
    {
        public static float burrowDistance = 256f;
        public static float duration = 0.8f;
        public static float delayDuration = 0.4f;
        public static float exitDuration = 1.25f;

        private Transform modelTransform;
        private Vector3 burrowDestination = Vector3.zero;
        private Vector3 burrowStart = Vector3.zero;
        private Animator animator;
        private CharacterModel characterModel;
        private HurtBoxGroup hurtboxGroup;
        private ChildLocator childLocator;
        private bool isExiting;
        private bool hasBurrowed;
        private bool hasFinishedBurrow;

        public override void OnEnter()
        {
            base.OnEnter();
            base.StartAimMode(2f, false);
            this.modelTransform = base.GetModelTransform();
            if (this.modelTransform)
            {
                this.animator = this.modelTransform.GetComponent<Animator>();
                this.characterModel = this.modelTransform.GetComponent<CharacterModel>();
                this.hurtboxGroup = this.modelTransform.GetComponent<HurtBoxGroup>();
                this.childLocator = this.modelTransform.GetComponent<ChildLocator>();
            }

            if (base.characterMotor)
            {
                base.characterMotor.enabled = false;
            }

            base.gameObject.layer = LayerIndex.fakeActor.intVal;
            base.characterMotor.Motor.RebuildCollidableLayers();

            this.CalculateBurrowDestination();

            base.PlayAnimation("FullBody, Override", "Burrow", "Burrow.playbackRate", Burrow.duration - Burrow.delayDuration);
        }

        private void CalculateBurrowDestination()
        {
            Vector3 vector = Vector3.zero;
            Ray aimRay = base.GetAimRay();

            BullseyeSearch bullseyeSearch = new BullseyeSearch();
            bullseyeSearch.searchOrigin = aimRay.origin;
            bullseyeSearch.searchDirection = aimRay.direction;
            bullseyeSearch.maxDistanceFilter = Burrow.burrowDistance;
            bullseyeSearch.teamMaskFilter = TeamMask.allButNeutral;
            bullseyeSearch.filterByLoS = false;
            bullseyeSearch.teamMaskFilter.RemoveTeam(TeamComponent.GetObjectTeam(base.gameObject));
            bullseyeSearch.sortMode = BullseyeSearch.SortMode.Angle;
            bullseyeSearch.RefreshCandidates();

            HurtBox hurtBox = bullseyeSearch.GetResults().FirstOrDefault<HurtBox>();
            if (hurtBox)
            {
                vector = hurtBox.transform.position - base.transform.position;
            }

            this.burrowDestination = base.transform.position;
            this.burrowStart = base.transform.position;

            NodeGraph groundNodes = SceneInfo.instance.groundNodes;
            NodeGraph.NodeIndex nodeIndex = groundNodes.FindClosestNode(base.transform.position + vector, base.characterBody.hullClassification);
            groundNodes.GetNodePosition(nodeIndex, out this.burrowDestination);

            this.burrowDestination += base.transform.position - base.characterBody.footPosition;
            base.characterDirection.forward = vector;
        }

        private void CreateBurrowEffect(Vector3 origin)
        {
            EffectData effectData = new EffectData();
            effectData.rotation = this.modelTransform.rotation;
            effectData.origin = origin;

            EffectManager.SpawnEffect(EntityStates.MiniMushroom.InPlant.burrowPrefab, effectData, false);
        }

        private void SetPosition(Vector3 newPosition)
        {
            if (base.characterMotor)
            {
                base.characterMotor.Motor.SetPositionAndRotation(newPosition, Quaternion.identity, true);
            }
        }

        public override void FixedUpdate()
        {
            base.FixedUpdate();

            if (base.characterMotor)
            {
                base.characterMotor.velocity = Vector3.zero;
            }

            if (base.fixedAge >= Burrow.duration - Burrow.delayDuration && !this.hasBurrowed)
            {
                this.hasBurrowed = true;

                Util.PlaySound(EntityStates.MiniMushroom.InPlant.burrowInSoundString, base.gameObject);
                this.CreateBurrowEffect(Util.GetCorePosition(base.gameObject));
            }

            if (base.fixedAge >= Burrow.duration)
            {
                if (!this.hasFinishedBurrow)
                {
                    this.hasFinishedBurrow = true;
                    this.ExitCleanup();

                    base.PlayAnimation("FullBody, Override", "BufferEmpty");
                    base.PlayAnimation("Body", "Spawn", "Spawn.playbackRate", Burrow.exitDuration);

                    this.SetPosition(this.burrowDestination);
                    Util.PlaySound(EntityStates.MiniMushroom.UnPlant.UnplantOutSoundString, base.gameObject);
                    this.CreateBurrowEffect(this.burrowDestination);
                }
            }

            if (base.fixedAge >= Burrow.duration + Burrow.exitDuration && base.isAuthority)
            {
                this.outer.SetNextStateToMain();
            }
        }

        private void ExitCleanup()
        {
            if (this.isExiting)
            {
                return;
            }

            this.isExiting = true;
            base.gameObject.layer = LayerIndex.defaultLayer.intVal;
            base.characterMotor.Motor.RebuildCollidableLayers();

            this.modelTransform = base.GetModelTransform();

            if (base.characterMotor)
            {
                base.characterMotor.enabled = true;
            }
        }

        public override void OnExit()
        {
            base.OnExit();
            this.ExitCleanup();
        }
    }
}
