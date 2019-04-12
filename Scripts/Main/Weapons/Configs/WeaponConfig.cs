using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Main.Weapons.API;
using Main.Bullets.API;

namespace Main.Weapons.Configs
{
    [CreateAssetMenu(fileName = "WeaponConfig", menuName = "Config/WeaponConfig", order = 2)]
    public class WeaponConfig : ScriptableObject
    {
        #region Common
        [Header("Common")]
        public WeaponType weaponName;
        [Range(0, 10)]
        public float damageFactor;
        public float shootKd;
        public float bulletStartSpeed;
        public float mass;
        public float reloadKd;
        public int magazineBulletsAmount;
        public int extendedMagazin;
        public BulletType bulletType;
        public string aimTargetPrefab;
        #endregion
    }
}
