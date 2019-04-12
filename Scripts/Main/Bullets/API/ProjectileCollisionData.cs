using Core.MessageBus;
using Core.Services;
using UnityEngine;

namespace Main.Bullets.API
{
    public class ProjectileCollisionData : MessageData
    {
        private static ObjectPool<ProjectileCollisionData> _pool = new ObjectPool<ProjectileCollisionData>();
        
        public int BulletId;
        public Vector3 Position;
        public Vector3 Normal;
        
        public ProjectileCollisionData() {}

        public static ProjectileCollisionData GetProjectileCollisionData
            (int bulletId, Vector3 position, Vector3 normal)
        {
            var data = _pool.Get();
            data.BulletId = bulletId;
            data.Position = position;
            data.Normal = normal;
            return data;
        }
        
        public override void FreeObjectInPool()
        {
            _pool.Release(this);
        }
        
        public override MessageData GetCopy()
        {
            var data = _pool.Get();
            data.BulletId = BulletId;
            data.Position = Position;
            data.Normal = Normal;
            return data;
        }
    }
}