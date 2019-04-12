using System.Collections;
using Core.MessageBus;
using Main.Bullets;
using Main.Bullets.API;
using Main.Looting.API;
using Main.Weapons.Data;
using UnityEngine;

namespace Main.Weapons.Components
{
    public class AutoShootingComponent : SubscriberBehaviour
    {
        private WeaponData _weaponData;

        private float _kdTimer;

        protected override void Awake()
        {
            base.Awake();
            _weaponData = gameObject.GetComponent<WeaponData>();
        }

        [Subscribe(SubscribeType.Channel, API.Messages.SHOOTING)]
        private void Shoot(Message msg)
        {
            if (_kdTimer <= 0)
            {
                if (_weaponData.MagazineBulletAmount > 0)
                {
                    --_weaponData.MagazineBulletAmount;

                    _kdTimer = _weaponData.WeaponConfig.shootKd;

                    //ToDo: replace to server
                    var id = 1000 * _weaponData.OwnerNetId + BulletsFactory.GetNextBulletId();

                    MessageBus.SendMessage(NetBroadcastMessage.Get(Network.API.Messages.CREATE_PROJECTILE, UnityEngine.Networking.QosType.Reliable, -1,
                        ProjectileMessageData.GetProjectileMessageData(
                            id,
                            _weaponData.OwnerNetId,
                            _weaponData.ShootPosition.position,
                            _weaponData.ShootPosition.rotation,
                            _weaponData.WeaponConfig.bulletType,
                            _weaponData.WeaponConfig.bulletStartSpeed,
                            _weaponData.WeaponConfig.damageFactor)));

                    MessageBus.SendMessage(SubscribeType.Channel, Channel.ChannelIds[SubscribeType.Channel],
                        CommonMessage.Get(API.Messages.FIRING));

                }
                else
                {
                    _weaponData.NoBulletsInMagazine();
                }
            }
        }

        private void Update()
        {
            _kdTimer -= Time.deltaTime;
        }

        void OnDisable()
        {
            Unsubscribe();
        }

        void OnEnable()
        {
            Subscribe();
        }
    }
}