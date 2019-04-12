using System;
using Core.Services;

namespace Core.MessageBus.MessageDataTemplates
{
    [Serializable]
    public class FloatData: MessageData
    {
        public float Value;
        
        private static ObjectPool<FloatData> _pool = new ObjectPool<FloatData>();

        public FloatData()
        {
            Value = 0;
        }

        public FloatData(float value)
        {
            Value = value;
        }

        public static FloatData GetPooledObject()
        {
            return _pool.Get();
        }
        
        public static FloatData GetFloatData(float value)
        {
            var data = _pool.Get();
            data.Value = value;

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