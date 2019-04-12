using System.Collections.Generic;
using UnityEngine;

namespace Core.Services
{
    public class ObjectPool<T> where T : new()
    {
        private readonly List<T> _pool = new List<T>();
        
        private readonly float _increaseFactor;
        private int _curSize;

        public ObjectPool(int startSize = 10, float increaseFactor = 1.6f)
        {
            _increaseFactor = increaseFactor;
            _curSize = startSize;
            
            for (var i = 0; i < startSize; ++i)
            {
                _pool.Add(new T());
            }
        }

        private void IncreasePool()
        {
            var prevSize = _curSize;
            _curSize = (int)(_curSize * _increaseFactor);
            
            for (var i = prevSize; i < _curSize; ++i)
            {
                _pool.Add(new T());
            }
        }
        
        public T Get()
        {
            if (_pool.Count == 0) IncreasePool();
            
            lock (_pool)
            {
                var obj = _pool[0];
                _pool.RemoveAt(0);
                return obj;
            }
        }

        public void Release(T obj)
        {
            lock (_pool)
            {
                _pool.Add(obj);
            }
        }
    }
}