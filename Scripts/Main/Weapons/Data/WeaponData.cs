using System.Collections;
using Core.MessageBus;
using Core.MessageBus.MessageDataTemplates;
using Main.Inventory.Data;
using UnityEngine;
using CameraTargetConfig = Main.Weapons.Configs.CameraTargetConfig;
using SpreadConfig = Main.Weapons.Configs.SpreadConfig;
using WeaponConfig = Main.Weapons.Configs.WeaponConfig;

namespace Main.Weapons.Data
{
    public class WeaponData : SubscriberBehaviour
    {
        public WeaponConfig WeaponConfig;
        public SpreadConfig SpreadConfig;
        public CameraTargetConfig CamTargetConfig;

        public Transform ShootPosition;
        public Transform HandTarget;
        
        public bool AimingIK = false;
        
        //ToDo: replace hard coherence to interface
        public InventoryData Data;

        public int OwnerNetId;

        private int _magazineBulletAmount;
        
        public int MagazineBulletAmount
        {
            get { return _magazineBulletAmount; }
            set
            {
                _magazineBulletAmount = value;
                
                MessageBus.SendMessage(SubscribeType.Channel, Channel.ChannelIds[SubscribeType.Channel],
                    CommonMessage.Get(API.Messages.UPDATE_MAGAZINE_BULLETS_AMOUNT, 
                        IntData.GetIntData(_magazineBulletAmount)));
            }
        }

        public void NoBulletsInMagazine()
        {
            MessageBus.SendMessage(SubscribeType.Channel, Channel.ChannelIds[SubscribeType.Channel],
                CommonMessage.Get(API.Messages.NO_BULLETS_IN_MAGAZINE));
        }

        protected override void Awake()
        {
        }

        public override void AfterAction()
        {
            base.AfterAction();

            StartCoroutine(SendMsgs());
        }

        private IEnumerator SendMsgs()
        {
            yield return new WaitForEndOfFrame();

            if (Channel == null)
                Channel = GetComponent<Channel>();

            MessageBus.SendMessage(SubscribeType.Channel, Channel.ChannelIds[SubscribeType.Channel], CommonMessage.Get(
                Inventory.API.Messages.CHECK_UPDATE), true);
            Debug.Log("send UPDATE_SHOOT_POSITION_TRANSFORM message");
            MessageBus.SendMessage(SubscribeType.Channel, Channel.ChannelIds[SubscribeType.Channel], CommonMessage.Get(
                AimTarget.API.Messages.UPDATE_SHOOT_POSITION_TRANSFORM,
                ObjectData.GetObjectData(ShootPosition)), true);
            MessageBus.SendMessage(SubscribeType.Channel, Channel.ChannelIds[SubscribeType.Channel], CommonMessage.Get(
                AimTarget.API.Messages.UPDATE_SPREAD_CONFIG,
                ObjectData.GetObjectData(SpreadConfig)), true);
        }
        
        [Subscribe(SubscribeType.Channel,Inventory.API.Messages.UPDATE)]
        private void UpdateInventoryData(Message msg)
        {
            Data = ObjectData.ParseObjectData<InventoryData>(msg.Data);
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