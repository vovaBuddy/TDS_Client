using System.Collections.Generic;
using Core.MessageBus;
using Core.Services;
using Main.Bullets.API;
using Main.Bullets.Data;
using UnityEngine;

namespace Main.Bullets
{
    public class BulletsFactory : SubscriberBehaviour
    {
        public static BulletsFactory Instance;

        private static int _bulletId = -1;
        public static int GetNextBulletId()
        {
            return ++_bulletId;
        }

        protected override void Awake()
        {
            base.Awake();
            
            Instance = this;
        }

        [Subscribe(API.Messages.PROJECTILE_COLLISION)]
        private void Collision(Message msg)
        {
            var data = (ProjectileCollisionData) msg.Data;
            
            ServiceLocator.GetService<ResourceLoaderService>().InstantiatePrefabByPathName("VFX/Bullets/SmallRedImpact", 
                (go =>
                {
                    go.transform.position = data.Position;
                    go.transform.rotation = Quaternion.LookRotation(data.Normal);
                }));
        }

        [Subscribe(SubscribeType.Broadcast, Network.API.Messages.CREATE_PROJECTILE, 
            Network.API.Messages.CREATE_PROJECTILE_REPLICANT)]
        private void CreateProjectile(Message msg)
        {
            var projectileData = (msg.Data as API.ProjectileMessageData);

            var postfix = msg is CommonMessage ? "_Replicant" : string.Empty;

            Debug.Log("CreateProjectile " + projectileData.type.ToString() + postfix);
            
            ServiceLocator.GetService<ResourceLoaderService>()
                .InstantiatePrefabByPathName("Projectiles/Bullets/" + projectileData.type.ToString() + postfix, 
                    (go =>
                    {
                        go.transform.position = projectileData.GetPostition();
                        go.transform.rotation = projectileData.GetQuaternion();
                        go.GetComponent<ProjectileData>().DamageFactor = projectileData.damageFactor;
                        go.GetComponent<Rigidbody>().velocity = go.transform.forward * projectileData.startSpeed;
                        go.GetComponent<ProjectileData>().BulletId = projectileData.BulletNetId;

                        var sub = go.GetComponent<SubscriberBehaviour>();
                        var channel = go.AddComponent<Channel>();
                        channel.ChannelIds.Add(SubscribeType.Network, projectileData.BulletNetId);
                        sub.ReSubscribe();
                    }));
        }

    }
}