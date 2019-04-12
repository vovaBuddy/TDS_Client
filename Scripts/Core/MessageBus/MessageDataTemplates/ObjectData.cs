using System;
using Core.Services;

namespace Core.MessageBus.MessageDataTemplates
{
    public class ObjectData : MessageData
    {
        public object Value;
        
        private static ObjectPool<ObjectData> _pool = new ObjectPool<ObjectData>(5);

        public ObjectData()
        {
            Value = 0;
        }

        public ObjectData(object value)
        {
            Value = value;
        }

        public static ObjectData GetPooledObject()
        {
            return _pool.Get();
        }
        
        public static ObjectData GetObjectData(object value)
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
        
        public static T ParseObjectData<T>(MessageData data) where T : class 
        {
            return (data as ObjectData).Value as T;
        }

        public static T ParseData<T>(MessageData data) where T : class
        {
            return (data as T);
        }
    }
}