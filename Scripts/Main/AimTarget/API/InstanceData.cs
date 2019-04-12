using UnityEngine;
using System.Collections;
using System;
using Core.MessageBus;
using Core.Services;

namespace Main.AimTarget.API
{
    [Serializable]
    public class InstanceData : MessageData
    {
        public string Type;
        public int[] NetIds;
        public int CoupledObjectNetId;

        public InstanceData()
        {
            Type = string.Empty;
            NetIds = null;
            CoupledObjectNetId = -1;
        }

        public static InstanceData GetData(string type, int netId, int coupledNetId = -1)
        {
            var data = _pool.Get();
            data.Type = type;
            data.NetIds = new int[1] { netId };
            data.CoupledObjectNetId = coupledNetId;

            return data;
        }

        public static InstanceData GetData(string type, int[] netIds, int coupledNetId = -1)
        {
            var data = _pool.Get();
            data.Type = type;
            data.NetIds = netIds;
            data.CoupledObjectNetId = coupledNetId;

            return data;
        }

        private static ObjectPool<InstanceData> _pool = new ObjectPool<InstanceData>(1);

        public override void FreeObjectInPool()
        {
            _pool.Release(this);
        }

        public override MessageData GetCopy()
        {
            var data = _pool.Get();
            data.Type = Type;
            data.NetIds = NetIds;
            data.CoupledObjectNetId = CoupledObjectNetId;

            return data;
        }
    }
}