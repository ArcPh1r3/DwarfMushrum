using System.Reflection;
using R2API;
using UnityEngine;
using UnityEngine.Networking;
using RoR2;

namespace DwarfMushrum.Modules
{
    public static class Assets
    {
        public static AssetBundle mainAssetBundle;

        public static Texture charPortrait;

        //public static Sprite iconP;
        //public static Sprite icon1;
        //public static Sprite icon2;
        //public static Sprite icon3;
        //public static Sprite icon4;

        public static void PopulateAssets()
        {
            if (mainAssetBundle == null)
            {
                using (var assetStream = Assembly.GetExecutingAssembly().GetManifestResourceStream("DwarfMushrum.dwarfmushrum"))
                {
                    mainAssetBundle = AssetBundle.LoadFromStream(assetStream);
                    var provider = new AssetBundleResourcesProvider("@DwarfMushrum", mainAssetBundle);
                    ResourcesAPI.AddProvider(provider);
                }
            }

            //charPortrait = mainAssetBundle.LoadAsset<Sprite>("texDwarfMushrumIcon").texture;

            //iconP = mainAssetBundle.LoadAsset<Sprite>("");
            //icon1 = mainAssetBundle.LoadAsset<Sprite>("");
            //icon2 = mainAssetBundle.LoadAsset<Sprite>("");
            //icon3 = mainAssetBundle.LoadAsset<Sprite>("");
            //icon4 = mainAssetBundle.LoadAsset<Sprite>("");

            //Shader hotpoo = Resources.Load<Shader>("Shaders/Deferred/hgstandard");
        }

        private static GameObject LoadEffect(string resourceName, string soundName)
        {
            GameObject newEffect = mainAssetBundle.LoadAsset<GameObject>(resourceName);

            newEffect.AddComponent<DestroyOnTimer>().duration = 12;
            newEffect.AddComponent<NetworkIdentity>();
            newEffect.AddComponent<VFXAttributes>().vfxPriority = VFXAttributes.VFXPriority.Always;
            var effect = newEffect.AddComponent<EffectComponent>();
            effect.applyScale = false;
            effect.effectIndex = EffectIndex.Invalid;
            effect.parentToReferencedTransform = true;
            effect.positionAtReferencedTransform = true;
            effect.soundName = soundName;

            EffectAPI.AddEffect(newEffect);

            return newEffect;
        }

        public static Material CreateMaterial(string materialName, float emission, Color emissionColor, float normalStrength)
        {
            if (!DwarfMushrumPlugin.commandoMat) DwarfMushrumPlugin.commandoMat = Resources.Load<GameObject>("Prefabs/CharacterBodies/CommandoBody").GetComponentInChildren<CharacterModel>().baseRendererInfos[0].defaultMaterial;

            Material mat = UnityEngine.Object.Instantiate<Material>(DwarfMushrumPlugin.commandoMat);
            Material tempMat = Assets.mainAssetBundle.LoadAsset<Material>(materialName);
            if (!tempMat)
            {
                Debug.LogError(materialName + " does not exist you fucking dumbass");
                return DwarfMushrumPlugin.commandoMat;
            }

            mat.SetColor("_Color", tempMat.GetColor("_Color"));
            mat.SetTexture("_MainTex", tempMat.GetTexture("_MainTex"));
            mat.SetColor("_EmColor", emissionColor);
            mat.SetFloat("_EmPower", emission);
            mat.SetTexture("_EmTex", tempMat.GetTexture("_EmissionMap"));
            mat.SetFloat("_NormalStrength", normalStrength);

            return mat;
        }
    }
}
