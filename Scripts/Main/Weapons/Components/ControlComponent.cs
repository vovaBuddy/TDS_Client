using System.Collections;
using Core.MessageBus;
using Main.Bullets;
using Main.Bullets.API;
using Main.Looting.API;
using Main.Weapons.Data;
using UnityEngine;

namespace Main.Weapons.Components
{
    public class ControlComponent : SubscriberBehaviour
    {
        private WeaponData _weaponData;
        
        protected override void Awake()
        {
            base.Awake();
            _weaponData = gameObject.GetComponent<WeaponData>();
        }
        
        [Subscribe(SubscribeType.Channel,API.Messages.START_RELOAD)]
        private void Reload(Message msg)
        {
            var lootFullName = Looting.Data.LootData.GetFullLootName(LootType.BULLETS, new int[] { (int)_weaponData.WeaponConfig.bulletType });

            Debug.Log("Reload: " + lootFullName);

            _weaponData.MagazineBulletAmount += _weaponData.Data.GetNeededAmountByType(
                _weaponData.WeaponConfig.magazineBulletsAmount - _weaponData.MagazineBulletAmount,
                lootFullName);

            StartCoroutine(Reload(_weaponData.WeaponConfig.reloadKd));
        }

        private IEnumerator Reload(float time)
        {
            yield return new WaitForSeconds(time);
            
            MessageBus.SendMessage(SubscribeType.Channel, Channel.ChannelIds[SubscribeType.Channel],
                CommonMessage.Get(API.Messages.END_RELOAD));
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