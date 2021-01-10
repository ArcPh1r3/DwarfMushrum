using BepInEx;
using EntityStates;
using R2API;
using R2API.Utils;
using RoR2;
using RoR2.CharacterAI;
using System.Collections.Generic;
using UnityEngine;

namespace DwarfMushrum.Modules
{
    public static class Prefabs
    {
        public static GameObject bodyPrefab;
        public static GameObject masterPrefab;

        public static void CreatePrefab()
        {
            bodyPrefab = PrefabAPI.InstantiateClone(Resources.Load<GameObject>("Prefabs/CharacterBodies/CommandoBody"), "DwarfMushrumBody");

            CharacterBody bodyComponent = bodyPrefab.GetComponent<CharacterBody>();

            bodyComponent.name = "DwarfMushrumBody";
            bodyComponent.baseNameToken = "DWARFMUSHRUM_BODY_NAME";
            bodyComponent.subtitleNameToken = "DWARFMUSHRUM_BODY_SUBTITLE";
            bodyComponent.baseMoveSpeed = 0f;
            bodyComponent.baseMaxHealth = 160f;
            bodyComponent.levelMaxHealth = 48f;
            bodyComponent.baseDamage = 8f;
            bodyComponent.levelDamage = bodyComponent.baseDamage * 0.2f;
            bodyComponent.isChampion = false;
            bodyComponent.baseJumpCount = 0;
            //bodyComponent.portraitIcon = Assets.bossPortrait;

            //sfx
            var sfx = bodyPrefab.GetComponent<SfxLocator>();
            sfx.barkSound = "Play_minimushroom_idle_VO";
            sfx.deathSound = "Play_minimushroom_death";
            sfx.fallDamageSound = "";
            sfx.landingSound = "Play_minimushroom_step";

            //
            SetupModel();

            masterPrefab = PrefabAPI.InstantiateClone(Resources.Load<GameObject>("Prefabs/CharacterMasters/SquidTurretMaster"), "DwarfMushrumMaster");

            CharacterMaster master = masterPrefab.GetComponent<CharacterMaster>();

            CreateAI();

            master.bodyPrefab = bodyPrefab;
            master.isBoss = false;

            BodyCatalog.getAdditionalEntries += delegate (List<GameObject> list)
            {
                list.Add(bodyPrefab);
            };

            MasterCatalog.getAdditionalEntries += delegate (List<GameObject> list)
            {
                list.Add(masterPrefab);
            };
        }

        private static void CreateAI()
        {
            foreach (AISkillDriver ai in masterPrefab.GetComponentsInChildren<AISkillDriver>())
            {
                BaseUnityPlugin.DestroyImmediate(ai);
            }

            AISkillDriver deathDriver = masterPrefab.AddComponent<AISkillDriver>();
            deathDriver.customName = "Burrow";
            deathDriver.movementType = AISkillDriver.MovementType.ChaseMoveTarget;
            deathDriver.moveTargetType = AISkillDriver.TargetType.CurrentEnemy;
            deathDriver.activationRequiresAimConfirmation = true;
            deathDriver.activationRequiresTargetLoS = false;
            deathDriver.selectionRequiresTargetLoS = true;
            deathDriver.maxDistance = 512f;
            deathDriver.minDistance = 35f;
            deathDriver.requireSkillReady = true;
            deathDriver.aimType = AISkillDriver.AimType.None;
            deathDriver.ignoreNodeGraph = false;
            deathDriver.moveInputScale = 1f;
            deathDriver.driverUpdateTimerOverride = -1f;
            deathDriver.buttonPressType = AISkillDriver.ButtonPressType.Hold;
            deathDriver.minTargetHealthFraction = Mathf.NegativeInfinity;
            deathDriver.maxTargetHealthFraction = Mathf.Infinity;
            deathDriver.minUserHealthFraction = Mathf.NegativeInfinity;
            deathDriver.maxUserHealthFraction = Mathf.Infinity;
            deathDriver.skillSlot = SkillSlot.Utility;

            /*AISkillDriver hideDriver = masterPrefab.AddComponent<AISkillDriver>();
            hideDriver.customName = "Hide";
            hideDriver.movementType = AISkillDriver.MovementType.Stop;
            hideDriver.moveTargetType = AISkillDriver.TargetType.CurrentEnemy;
            hideDriver.activationRequiresAimConfirmation = true;
            hideDriver.activationRequiresTargetLoS = false;
            hideDriver.selectionRequiresTargetLoS = true;
            hideDriver.maxDistance = 5f;
            hideDriver.minDistance = 0f;
            hideDriver.requireSkillReady = true;
            hideDriver.aimType = AISkillDriver.AimType.AtCurrentEnemy;
            hideDriver.ignoreNodeGraph = false;
            hideDriver.moveInputScale = 1f;
            hideDriver.driverUpdateTimerOverride = -1f;
            hideDriver.buttonPressType = AISkillDriver.ButtonPressType.Hold;
            hideDriver.minTargetHealthFraction = Mathf.NegativeInfinity;
            hideDriver.maxTargetHealthFraction = Mathf.Infinity;
            hideDriver.minUserHealthFraction = Mathf.NegativeInfinity;
            hideDriver.maxUserHealthFraction = Mathf.Infinity;
            hideDriver.skillSlot = SkillSlot.Secondary;*/

            AISkillDriver shootDriver = masterPrefab.AddComponent<AISkillDriver>();
            shootDriver.customName = "Shoot";
            shootDriver.movementType = AISkillDriver.MovementType.ChaseMoveTarget;
            shootDriver.moveTargetType = AISkillDriver.TargetType.CurrentEnemy;
            shootDriver.activationRequiresAimConfirmation = true;
            shootDriver.activationRequiresTargetLoS = false;
            shootDriver.selectionRequiresTargetLoS = true;
            shootDriver.maxDistance = 32;
            shootDriver.minDistance = 0f;
            shootDriver.requireSkillReady = true;
            shootDriver.aimType = AISkillDriver.AimType.AtCurrentEnemy;
            shootDriver.ignoreNodeGraph = false;
            shootDriver.moveInputScale = 1f;
            shootDriver.driverUpdateTimerOverride = -1f;
            shootDriver.buttonPressType = AISkillDriver.ButtonPressType.Hold;
            shootDriver.minTargetHealthFraction = Mathf.NegativeInfinity;
            shootDriver.maxTargetHealthFraction = Mathf.Infinity;
            shootDriver.minUserHealthFraction = Mathf.NegativeInfinity;
            shootDriver.maxUserHealthFraction = Mathf.Infinity;
            shootDriver.skillSlot = SkillSlot.Primary;

            AISkillDriver idleDriver = masterPrefab.AddComponent<AISkillDriver>();
            idleDriver.customName = "Idle";
            idleDriver.movementType = AISkillDriver.MovementType.ChaseMoveTarget;
            idleDriver.moveTargetType = AISkillDriver.TargetType.CurrentEnemy;
            idleDriver.activationRequiresAimConfirmation = false;
            idleDriver.activationRequiresTargetLoS = false;
            idleDriver.selectionRequiresTargetLoS = false;
            idleDriver.maxDistance = 512f;
            idleDriver.minDistance = 0f;
            idleDriver.requireSkillReady = true;
            idleDriver.aimType = AISkillDriver.AimType.AtCurrentEnemy;
            idleDriver.ignoreNodeGraph = false;
            idleDriver.moveInputScale = 1f;
            idleDriver.driverUpdateTimerOverride = -1f;
            idleDriver.buttonPressType = AISkillDriver.ButtonPressType.Abstain;
            idleDriver.minTargetHealthFraction = Mathf.NegativeInfinity;
            idleDriver.maxTargetHealthFraction = Mathf.Infinity;
            idleDriver.minUserHealthFraction = Mathf.NegativeInfinity;
            idleDriver.maxUserHealthFraction = Mathf.Infinity;
            idleDriver.skillSlot = SkillSlot.None;
        }

        private static GameObject CreateModel(GameObject main)
        {
            if (main.transform.Find("ModelBase")) DwarfMushrumPlugin.Destroy(main.transform.Find("ModelBase").gameObject);
            if (main.transform.Find("CameraPivot"))  DwarfMushrumPlugin.Destroy(main.transform.Find("CameraPivot").gameObject);
            if (main.transform.Find("AimOrigin"))  DwarfMushrumPlugin.Destroy(main.transform.Find("AimOrigin").gameObject);

            GameObject model = null;

            model = Modules.Assets.mainAssetBundle.LoadAsset<GameObject>("mdlDwarfMushrum");

            return model;
        }

        private static void SetupModel()
        {
            GameObject model = CreateModel(bodyPrefab);

            GameObject gameObject = new GameObject("ModelBase");
            gameObject.transform.parent = bodyPrefab.transform;
            gameObject.transform.localPosition = new Vector3(0f, -0.81f, 0f);
            gameObject.transform.localRotation = Quaternion.identity;
            gameObject.transform.localScale = new Vector3(1f, 1f, 1f);

            GameObject gameObject2 = new GameObject("CameraPivot");
            gameObject2.transform.parent = bodyPrefab.transform;
            gameObject2.transform.localPosition = new Vector3(0f, 1.6f, 0f);
            gameObject2.transform.localRotation = Quaternion.identity;
            gameObject2.transform.localScale = Vector3.one;

            GameObject gameObject3 = new GameObject("AimOrigin");
            gameObject3.transform.parent = bodyPrefab.transform;
            gameObject3.transform.localPosition = new Vector3(0f, 0.5f, 0f);
            gameObject3.transform.localRotation = Quaternion.identity;
            gameObject3.transform.localScale = Vector3.one;

            Transform transform = model.transform;
            transform.parent = gameObject.transform;
            transform.localPosition = Vector3.zero;
            transform.localRotation = Quaternion.identity;

            CharacterDirection characterDirection = bodyPrefab.GetComponent<CharacterDirection>();
            characterDirection.moveVector = Vector3.zero;
            characterDirection.targetTransform = gameObject.transform;
            characterDirection.overrideAnimatorForwardTransform = null;
            characterDirection.rootMotionAccumulator = null;
            characterDirection.modelAnimator = model.GetComponentInChildren<Animator>();
            characterDirection.driveFromRootRotation = false;
            characterDirection.turnSpeed = 720f;

            ModelLocator modelLocator = bodyPrefab.GetComponent<ModelLocator>();
            modelLocator.modelTransform = transform;
            modelLocator.modelBaseTransform = gameObject.transform;

            ChildLocator childLocator = model.GetComponent<ChildLocator>();

            CharacterModel characterModel = model.AddComponent<CharacterModel>();
            characterModel.body = null;
            characterModel.baseRendererInfos = new CharacterModel.RendererInfo[]
            {
                new CharacterModel.RendererInfo
                {
                    defaultMaterial = Assets.CreateMaterial("matDwarfMushrum", 0.5f, Color.white, 0),
                    renderer = childLocator.FindChild("Model").GetComponent<SkinnedMeshRenderer>(),
                    defaultShadowCastingMode = UnityEngine.Rendering.ShadowCastingMode.On,
                    ignoreOverlays = false
                }
            };
            characterModel.autoPopulateLightInfos = true;
            characterModel.invisibilityCount = 0;
            characterModel.temporaryOverlays = new List<TemporaryOverlay>();
            characterModel.body = bodyPrefab.GetComponent<CharacterBody>();

            characterModel.SetFieldValue("mainSkinnedMeshRenderer", characterModel.baseRendererInfos[0].renderer.gameObject.GetComponent<SkinnedMeshRenderer>());

            CreateHurtbox(model, bodyPrefab.GetComponent<HealthComponent>(), childLocator);

            AimAnimator aimAnimator = model.AddComponent<AimAnimator>();
            aimAnimator.directionComponent = characterDirection;
            aimAnimator.pitchRangeMax = 60f;
            aimAnimator.pitchRangeMin = -60f;
            aimAnimator.yawRangeMin = -80f;
            aimAnimator.yawRangeMax = 80f;
            aimAnimator.pitchGiveupRange = 30f;
            aimAnimator.yawGiveupRange = 10f;
            aimAnimator.giveupDuration = 3f;
            aimAnimator.inputBank = bodyPrefab.GetComponent<InputBankTest>();
        }

        private static void CreateHurtbox(GameObject model, HealthComponent healthComponent, ChildLocator childLocator)
        {
            HurtBoxGroup hurtBoxGroup = model.AddComponent<HurtBoxGroup>();

            HurtBox mainHurtbox = childLocator.FindChild("MainHurtbox").gameObject.AddComponent<HurtBox>();
            mainHurtbox.gameObject.layer = LayerIndex.entityPrecise.intVal;
            mainHurtbox.healthComponent = healthComponent;
            mainHurtbox.isBullseye = true;
            mainHurtbox.damageModifier = HurtBox.DamageModifier.Normal;
            mainHurtbox.hurtBoxGroup = hurtBoxGroup;
            mainHurtbox.indexInGroup = 0;

            hurtBoxGroup.hurtBoxes = new HurtBox[]
            {
                mainHurtbox
            };

            hurtBoxGroup.mainHurtBox = mainHurtbox;
            hurtBoxGroup.bullseyeCount = 1;
        }
    }
}
