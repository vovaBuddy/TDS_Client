using System.Collections.Generic;
using Core.MessageBus;
using Core.MessageBus.MessageDataTemplates;
using UnityEngine;
using Main.Inventory.API;

namespace Main.Inventory.Data
{
    public class InventoryData : SubscriberBehaviour
    {
        public Dictionary<string, LootItemHolder> LootItemsHolder;
        public WeaponItemHolder[] WeaponItemHolders;
        public int CurSelectedWeaponIndex = -1;

        public int backpackMaxSlotAmount = 8;
        public int backpackEmptySlotAmount = 8;

        protected override void Awake()
        {
            base.Awake();

            LootItemsHolder = new Dictionary<string, LootItemHolder>();
            WeaponItemHolders = new WeaponItemHolder[2];
            WeaponItemHolders[0] = new WeaponItemHolder(null, Weapons.API.WeaponType.HEMLOK);
            WeaponItemHolders[1] = new WeaponItemHolder(null, Weapons.API.WeaponType.HEMLOK);
        }

        [Subscribe(API.Messages.WANT_OPEN_LOOT_MENU)]
        private void OpenLootMenu(Message msg)
        {
            MessageBus.SendMessage(CommonMessage.Get(API.Messages.OPEN_LOOT_MENU,
                ObjectData.GetObjectData(LootItemsHolder)));
        }

        public int GetNeededAmountByType(int amount, string type)
        {
            var result = Mathf.Min(amount, LootItemsHolder[type].Amount);
            LootItemsHolder[type].Amount -= result;

            if(LootItemsHolder[type].Amount <= (LootItemsHolder[type].SlotAmount - 1) * LootItemsHolder[type].MaxInSlot)
            {
                LootItemsHolder[type].SlotAmount -= 1;
            }

            Debug.Log("GetNeededAmountByType: " + result);
            
            ////ToDo: replace from here
            //var msgType = Inventory.API.Messages.UPDATE_PREFFIX +
            //              LootType.BULLETS + "_" + 1.ToString();
            
            //MessageBus.SendMessageToChannel(GetComponentInParent<MessageChannelID>(), 
            //    Message.GetMessage(msgType, IntData.GetIntData(InventoryItemsAmount[type])));

            return result;
        }
    }
}