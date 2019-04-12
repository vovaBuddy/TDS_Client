using System.Collections;
using Core.MessageBus;
using Main.Bullets;
using Main.Bullets.API;
using Main.Looting.API;
using Main.Weapons.Data;
using UnityEngine;

namespace Main.Weapons.Components
{
    public class BurstShootingComponent : SubscriberBehaviour
    {
        private WeaponData _weaponData;
        private bool _shooted;
        private bool _burstShooting;
        private int _burstAmount = 3;
        private int _curBurstAmount = 0;

        private float _kdTimer;

        protected override void Awake()
        {
            base.Awake();
            _weaponData = gameObject.GetComponent<WeaponData>();
            _shooted = false;
        }

        [Subscribe(SubscribeType.Channel, API.Messages.END_SHOOT)]
        private void Ready(Message msg)
        {
            _shooted = false;
        }

        [Subscribe(SubscribeType.Channel, API.Messages.SHOOT)]
        private void Shoot(Message msg)
        {
            if (_shooted) return;

            if (_burstShooting) return;

            _shooted = true;
            _burstShooting = true;

            _curBurstAmount = _burstAmount;

            Shoot();
        }

        private void Shoot()
        {
            if (_weaponData.MagazineBulletAmount > 0)
            {
                _curBurstAmount -= 1;
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
                _burstShooting = false;
            }
        }

        private void Update()
        {
            _kdTimer -= Time.deltaTime;

            if(_burstShooting)
            {
                if(_curBurstAmount == 0)
                {
                    _burstShooting = false;
                    return;
                }

                if(_kdTimer <= 0)
                {
                    Shoot();
                }
            }
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