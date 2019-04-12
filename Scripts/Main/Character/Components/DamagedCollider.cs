using System;
using Core.MessageBus;
using Main.Bullets.API;
using Main.Bullets.Data;
using Main.Characters.Data;
using UnityEngine;

namespace Main.Characters.Components
{
    public class DamagedCollider : MonoBehaviour, IOnCollisionComponent
    {
        private DamagedComponent _damagedComponent;
        private CharacterData _ownerCharacterData;
        
        public float DamageFactor;

        private void Awake()
        {
            _damagedComponent = GetComponentInParent<DamagedComponent>();
            _ownerCharacterData = gameObject.transform.parent.GetComponent<CharacterData>();
        }

        public bool IsProjectileOwner(int id)
        {
            return _damagedComponent.BulletIds.IndexOf(id) != -1;
        }

        public void OnCollisionAction(ProjectileData projectile, Collision collision)
        {
            var damage = 2 * DamageFactor;
            _ownerCharacterData.HitPoints -= (int)damage;

            MessageBus.SendMessage(NetBroadcastMessage.Get(Network.API.Messages.DELETE_OBJECT, UnityEngine.Networking.QosType.Reliable,
                projectile.GetComponent<Channel>().ChannelIds[SubscribeType.Network]));

            Destroy(projectile.gameObject);
        }
    }
}