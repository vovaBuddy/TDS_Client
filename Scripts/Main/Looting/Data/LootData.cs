using Main.Bullets.API;
using Main.Looting.API;
using System;
using UnityEngine;

namespace Main.Looting.Data
{
    [Serializable]
    public class LootData : MonoBehaviour
    {
        public static string GetFullLootName(BulletType bulletType)
        {
            return GetFullLootName(LootType.BULLETS, new int[1] { (int)bulletType });
        }

        public static string GetFullLootName(LootType type, int[] prms)
        {
            return type.ToString("d") + "_" + string.Join("", prms);
        }
        
        public int OwnerId;
        public LootType LootType;
        public int Amount;
        public int[] Params;

        public LootData(LootType lootType, int amount, int[] param)
        {
            LootType = lootType;
            Amount = amount;
            Params = param;
        }
    }
}