using System;
using Core.Services;

namespace Core.MessageBus.MessageDataTemplates
{
    [Serializable]
    public class IntData : MessageData
    {
        public int Value;
        
        private static ObjectPool<IntData> _pool = new ObjectPool<IntData>();

        public IntData()
        {
            Value = 0;
        }

        public IntData(int value)
        {
            Value = value;
        }

        public static IntData GetPooledObject()
        {
            return _pool.Get();
        }
        
        public static IntData GetIntData(int value)
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