using Core.MessageBus;
using Core.Services;
using UnityEngine;
using System;

namespace Main.Bullets.API
{
    [Serializable]
    public class ProjectileMessageData : MessageData
    {
        private static ObjectPool<ProjectileMessageData> _pool = new ObjectPool<ProjectileMessageData>(20);
        
        public int BulletNetId;
        public int OwnerNetId;

        private float X;
        private float Y;
        private float Z;

        private float Qx;
        private float Qy;
        private float Qz;
        private float Qw;

        public BulletType type;
        public float startSpeed;
        public float mass;
        public float damageFactor;

        public Vector3 GetPostition()
        {
            return new Vector3(X, Y, Z);
        }

        public Quaternion GetQuaternion()
        {
            return new Quaternion(Qx, Qy, Qz, Qw);
        }
        
        public ProjectileMessageData() {}

        public static ProjectileMessageData GetProjectileMessageData(
            int bulletNetId, int ownerNetId, Vector3 bulletPosition, Quaternion bulletRotation, BulletType type, 
            float startSpeed, float damageFactor)
        {
            var data = _pool.Get();

            data.OwnerNetId = ownerNetId;
            data.BulletNetId = bulletNetId;

            data.X = bulletPosition.x;
            data.Y = bulletPosition.y;
            data.Z = bulletPosition.z;

            data.Qx = bulletRotation.x;
            data.Qy = bulletRotation.y;
            data.Qz = bulletRotation.z;
            data.Qw = bulletRotation.w;

            data.type = type;
            data.startSpeed = startSpeed;
            data.damageFactor = damageFactor;

            return data;
        }
        
        public override void FreeObjectInPool()
        {
            _pool.Release(this);
        }
        
        public override MessageData GetCopy()
        {
            var data = _pool.Get();

            data.OwnerNetId = OwnerNetId;
            data.BulletNetId = BulletNetId;

            data.X = X;
            data.Y = Y;
            data.Z = Z;

            data.Qx = Qx;
            data.Qy = Qy;
            data.Qz = Qz;
            data.Qw = Qw;

            data.type = type;
            data.startSpeed = startSpeed;
            data.damageFactor = damageFactor;

            return data;
        }
    }
}