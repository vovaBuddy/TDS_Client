using System;
using Core.Services;
using UnityEngine;

namespace Core.MessageBus.MessageDataTemplates
{
    [Serializable]
    public class Vector3Data : MessageData
    {
        public Vector3 Value;
        
        private static ObjectPool<Vector3Data> _pool = new ObjectPool<Vector3Data>();

        public Vector3Data()
        {
            Value = Vector3.zero;
        }

        public Vector3Data(Vector3 value)
        {
            Value = value;
        }

        public static Vector3Data GetPooledObject()
        {
            return _pool.Get();
        }
        
        public static Vector3Data GetVector3Data(Vector3 vec)
        {
            var data = _pool.Get();
            data.Value = vec;

            return data;
        }
        
        public override void FreeObjectInPool()
        {
            _pool.Release(this);
        }
        
        public override MessageData GetCopy()
        {
            var data = _pool.Get();
            data.Value = Value;

            return data;
        }
    }
}