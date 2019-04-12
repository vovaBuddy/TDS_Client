using Core.MessageBus;
using Core.Services;
using Main.Bullets.API;
using Main.Bullets.Data;
using Main.Characters.Components;
using UnityEngine;

namespace Main.Bullets.Components
{
    public class CollisionComponent : SubscriberBehaviour
    {
        private ProjectileData _projectileData;

        protected override void Awake()
        {
            _projectileData = GetComponent<ProjectileData>();
        }

        private void OnCollisionEnter(Collision collision)
        {
            Debug.Log("Collision!");

            var collisioned = collision.gameObject.GetComponent<IOnCollisionComponent>();

            if (collisioned != null)
            {
                collisioned.OnCollisionAction(_projectileData, collision);
            }
        }

        [Subscribe(SubscribeType.Network, Network.API.Messages.DELETE_OBJECT)]
        private void delete(Message msg)
        {
            Destroy(gameObject);
        }
    }
}