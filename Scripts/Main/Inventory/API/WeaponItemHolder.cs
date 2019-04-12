using UnityEngine;
using System.Collections.Generic;

namespace Main.Inventory
{
    public class WeaponItemHolder
    {
        public GameObject WeaponObject;
        public GameObject AimTargetObject;
        public Weapons.API.WeaponType WeaponType;

        public WeaponItemHolder(GameObject obj, Weapons.API.WeaponType type)
        {
            WeaponObject = obj;
            WeaponType = type;
            AimTargetObject = null;
        }
    }
}