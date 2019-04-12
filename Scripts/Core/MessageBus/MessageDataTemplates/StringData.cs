using System;
using Core.Services;

namespace Core.MessageBus.MessageDataTemplates
{
    [Serializable]
    public class StringData : MessageData
    {
        public string Value;
        
        private static ObjectPool<StringData> _pool = new ObjectPool<StringData>();

        public StringData()
        {
            Value = string.Empty;
        }

        public StringData(string value)
        {
            Value = value;
        }

        public static StringData GetPooledObject()
        {
            return _pool.Get();
        }
        
        public static StringData GetStringData(string value)
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