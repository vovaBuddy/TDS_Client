using Core.MessageBus;
using Core.Services;
using System;
using UnityEngine;

namespace Core.MessageBus.MessageDataTemplates
{
    [Serializable]
    public class SyncTransformTimedData : MessageData
    {
        private float X;
        private float Y;
        private float Z;
        public float Time;

        public Vector3 GetVector3()
        {
            return new Vector3(X, Y, Z);
        }

        public SyncTransformTimedData()
        {
            X = 0;
            Y = 0;
            Z = 0;
            Time = 0;
        }

        public static SyncTransformTimedData GetData(Vector3 position, float time)
        {
            var data = _pool.Get();
            data.X = position.x;
            data.Y = position.y;
            data.Z = position.z;

            data.Time = time;

            return data;
        }

        private static ObjectPool<SyncTransformTimedData> _pool = new ObjectPool<SyncTransformTimedData>();

        public override void FreeObjectInPool()
        {
            _pool.Release(this);
        }

        public override MessageData GetCopy()
        {
            var data = _pool.Get();
            data.X = X;
            data.Y = Y;
            data.Z = Z;
            data.Time = Time;

            return data;
        }
    }
}