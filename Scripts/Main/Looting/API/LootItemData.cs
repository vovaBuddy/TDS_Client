using UnityEngine;
using System;
using Core.MessageBus;
using Main.Looting.Data;
using Core.Services;
using Main.Looting.API;

namespace Main.Looting
{
    [Serializable]
    public class LootItemData : MessageData
    {
        public LootType LootType;
        public int Amount;
        public int[] Params;

        private static ObjectPool<LootItemData> _pool = new ObjectPool<LootItemData>();

        public LootItemData() { }

        public static LootItemData GetData(LootType lootType, int amount, int[] prms)
        {
            var data = _pool.Get();
            data.LootType = lootType;
            data.Amount = amount;
            data.Params = prms;

            return data;
        }

        public override void FreeObjectInPool()
        {
            _pool.Release(this);
        }

        public override MessageData GetCopy()
        {
            var data = _pool.Get();
            data.LootType = LootType;
            data.Amount = Amount;
            data.Params = Params;

            return data;
        }
    }
}