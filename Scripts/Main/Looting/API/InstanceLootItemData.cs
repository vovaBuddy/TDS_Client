using UnityEngine;
using System;
using Core.MessageBus;
using Main.Looting.Data;
using Core.Services;
using Main.Looting.API;

namespace Main.Looting
{
    [Serializable]
    public class InstanceLootItemData : MessageData
    {
        public LootType LootType;
        public int Amout;
        public int[] Params;

        public int NetId;

        private float X;
        private float Y;
        private float Z;

        public Vector3 GetVector3()
        {
            return new Vector3(X, Y, Z);
        }

        public InstanceLootItemData()
        {
        }

        public static InstanceLootItemData GetData(Vector3 position, LootType lootType, int amount, int[] prms, int netId)
        {
            var data = _pool.Get();
            data.LootType = lootType;
            data.Amout = amount;
            data.Params = prms;

            data.NetId = netId;

            data.X = position.x;
            data.Y = position.y;
            data.Z = position.z;

            return data;
        }

        private static ObjectPool<InstanceLootItemData> _pool = new ObjectPool<InstanceLootItemData>();

        public override void FreeObjectInPool()
        {
            _pool.Release(this);
        }

        public override MessageData GetCopy()
        {
            var data = _pool.Get();
            data.LootType = LootType;
            data.Amout = Amout;
            data.Params = Params;

            data.NetId = NetId;

            data.X = X;
            data.Y = Y;
            data.Z = Z;

            return data;
        }
    }
}