using Main.Bullets.Data;
using UnityEngine;

namespace Main.Bullets.API
{
    public interface IOnCollisionComponent
    {
        void OnCollisionAction(ProjectileData projectile, Collision collision);
    }
}