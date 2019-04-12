using Core.MessageBus;
using Core.Services;
using Main.Bullets.API;
using Main.Bullets.Data;
using UnityEngine;

namespace Main.Environment.Component
{
    public class ObstacleComponent : MonoBehaviour, IOnCollisionComponent
    {
        public void OnCollisionAction(ProjectileData projectile, Collision collision)
        {
            ServiceLocator.GetService<ResourceLoaderService>().InstantiatePrefabByPathName("VFX/Bullets/SmallRedImpact",
            (go =>
            {
                go.transform.position = collision.contacts[0].point;
                go.transform.rotation = Quaternion.LookRotation(collision.contacts[0].normal);
            }));

            Destroy(projectile.gameObject);
        }
    }
}