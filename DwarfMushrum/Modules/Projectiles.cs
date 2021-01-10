using R2API;
using RoR2;
using RoR2.Projectile;
using UnityEngine;

namespace DwarfMushrum.Modules
{
    public static class Projectiles
    {
        public static GameObject sporeProjectile;

        public static void CreateProjectiles()
        {
            sporeProjectile = PrefabAPI.InstantiateClone(Resources.Load<GameObject>("Prefabs/Projectiles/Fireball"), "DwarfMushrumSpore", true);
            sporeProjectile.GetComponent<ProjectileController>().ghostPrefab = Resources.Load<GameObject>("Prefabs/Projectiles/SporeGrenadeProjectile").GetComponent<ProjectileController>().ghostPrefab;

            ProjectileCatalog.getAdditionalEntries += list =>
            {
                list.Add(sporeProjectile);
            };
        }
    }
}