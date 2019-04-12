using Core.MessageBus;
using Core.MessageBus.MessageDataTemplates;
using Main.Inventory.Data;
using Main.Weapons.API;
using UnityEngine;
using System.Collections.Generic;

namespace Main.Inventory.Components
{
    public class ControlComponent : SubscriberBehaviour
    {
        private InventoryData _inventoryData;
        private int curWeaopnSlotIndex = -1;


        private Dictionary<string, int> _maxAmountInSlot;

        protected override void Awake()
        {
            base.Awake();
            _inventoryData = gameObject.GetComponent<InventoryData>();
            _maxAmountInSlot = new Dictionary<string, int>();
            _maxAmountInSlot.Add(Looting.Data.LootData.GetFullLootName(Bullets.API.BulletType.HEAVY), 80);
            _maxAmountInSlot.Add(Looting.Data.LootData.GetFullLootName(Bullets.API.BulletType.SHELLS), 48);
        }

        [Subscribe(SubscribeType.Channel, API.Messages.CHECK_UPDATE)]
        private void Ping(Message msg)
        {
            MessageBus.SendMessage(SubscribeType.Channel, Channel.ChannelIds[SubscribeType.Channel],
                CommonMessage.Get(API.Messages.UPDATE,
                    ObjectData.GetObjectData(_inventoryData)));
        }

        [Subscribe(SubscribeType.Channel, API.Messages.PICK_UP_NEAR)]
        private void PickUpNear(Message msg)
        {
            Debug.Log("PICK UP!");

            MessageBus.SendMessage(NetServerMessage.Get(Network.API.Messages.TRY_PICK_UP_NEAR,
                UnityEngine.Networking.QosType.Reliable, Channel.ChannelIds[SubscribeType.Network]));
        }

        [Subscribe(SubscribeType.Network, Network.API.Messages.PICK_UP_LOOT_ITEM)]
        private void PickUp(Message msg)
        {
            var data = (Looting.LootItemData)msg.Data;

            Debug.Log("Pick up amount:" + data.Amount);

            MessageBus.SendMessage(SubscribeType.Channel, Channel.ChannelIds[SubscribeType.Channel],
                CommonMessage.Get(API.Messages.PICK_UP_PREFFIX + data.LootType.ToString(), msg.Data.GetCopy()));
        }
        
        [Subscribe(SubscribeType.Channel, API.Messages.PICK_UP_WEAPONS)]
        private void PickUpWeapon(Message msg)
        {
            bool hasEmpty = false;
            var data = (Looting.LootItemData)msg.Data;
            var PrevIndex = -1;

            if(_inventoryData.CurSelectedWeaponIndex == -1)
            {
                curWeaopnSlotIndex = 0;
            }
            else
            {
                curWeaopnSlotIndex = _inventoryData.CurSelectedWeaponIndex;
                PrevIndex = curWeaopnSlotIndex;

                for (int i = 0; i < _inventoryData.WeaponItemHolders.Length; ++i)
                {
                    if (_inventoryData.WeaponItemHolders[i].WeaponObject != null) continue;

                    hasEmpty = true;
                    curWeaopnSlotIndex = i;
                }

                if(!hasEmpty)
                {
                    //ToDo: drop current weapon
                    MessageBus.SendMessage(NetServerMessage.Get(Network.API.Messages.DROP_LOOT_ITEM, UnityEngine.Networking.QosType.Reliable,
                        -1, Looting.InstanceLootItemData.GetData(transform.position, Looting.API.LootType.WEAPONS, 1,
                        new int[1] {(int)_inventoryData.WeaponItemHolders[PrevIndex].WeaponObject.
                            GetComponent<Weapons.Data.WeaponData>().WeaponConfig.weaponName }, -1)));
                    Destroy(_inventoryData.WeaponItemHolders[PrevIndex].WeaponObject);
                    Destroy(_inventoryData.WeaponItemHolders[PrevIndex].AimTargetObject);
                }
                else
                {
                    _inventoryData.WeaponItemHolders[PrevIndex].WeaponObject.SetActive(false);
                }
            }

            MessageBus.SendMessage(NetBroadcastMessage.Get(Network.API.Messages.CREATE_WEAPON, UnityEngine.Networking.QosType.Reliable,
                Channel.ChannelIds[SubscribeType.Network], WeaponInstantiateData.GetWeaponInstantiateData((WeaponType)data.Params[0], null,
                Channel.ChannelIds[SubscribeType.Channel], -1, true)));
        }

        [Subscribe(SubscribeType.Channel, AimTarget.API.Messages.INSTANTIATED)]
        private void RegisterAimTarget(Message msg)
        {
            Debug.Log("RegisterAimTarget!");

            var aimObj = (msg.Data as ObjectData).Value as GameObject;

            if (_inventoryData.WeaponItemHolders[_inventoryData.CurSelectedWeaponIndex].WeaponObject != null)
            {
                _inventoryData.WeaponItemHolders[_inventoryData.CurSelectedWeaponIndex].AimTargetObject = aimObj;
            }
        }

        [Subscribe(SubscribeType.Channel, API.Messages.GET_NEXT_WEAPON)]
        private void NextWeapon(Message msg)
        {
            int weaponAmount = 0;
            int cur_index = -1;

            for(int i = 0; i < _inventoryData.WeaponItemHolders.Length; ++i)
            {
                if (_inventoryData.WeaponItemHolders[i].WeaponObject != null)
                {
                    weaponAmount++;

                    if(_inventoryData.WeaponItemHolders[i].WeaponObject.activeSelf)
                    {
                        cur_index = i;
                    }
                }
            }

            if (weaponAmount < 2 || cur_index == -1) return;

            _inventoryData.WeaponItemHolders[cur_index].WeaponObject.SetActive(false);
            _inventoryData.WeaponItemHolders[cur_index].AimTargetObject.SetActive(false);
            cur_index = (cur_index + 1) % _inventoryData.WeaponItemHolders.Length;
            _inventoryData.WeaponItemHolders[cur_index].WeaponObject.SetActive(true);
            _inventoryData.WeaponItemHolders[cur_index].AimTargetObject.SetActive(true);
            _inventoryData.CurSelectedWeaponIndex = cur_index;
        }

        [Subscribe(SubscribeType.Channel, Weapons.API.Messages.INSTANTIATED)]
        private void RegisterWeapon(Message msg)
        {
            if (curWeaopnSlotIndex == -1) return;

            _inventoryData.WeaponItemHolders[curWeaopnSlotIndex].WeaponObject =
                ((ObjectData)msg.Data).Value as GameObject;

            _inventoryData.CurSelectedWeaponIndex = curWeaopnSlotIndex;

            curWeaopnSlotIndex = -1;
        }

        [Subscribe(SubscribeType.Channel, API.Messages.PICK_UP_BULLETS, 
            API.Messages.PICK_UP_EQUIPMENTS)]
        private void PickUpBullets(Message msg)
        {
            var data = (Looting.LootItemData)msg.Data;
            var lootFullName = Looting.Data.LootData.GetFullLootName(data.LootType, data.Params);

            Debug.Log("Add bullets " + lootFullName);

            if (!_inventoryData.LootItemsHolder.ContainsKey(lootFullName))
            {
                if (_inventoryData.backpackEmptySlotAmount > 0)
                {
                    _inventoryData.backpackEmptySlotAmount -= 1;
                    _inventoryData.LootItemsHolder.Add(lootFullName, new LootItemHolder(data.Amount, 1, _maxAmountInSlot[lootFullName]));
                }
            }
            else
            {
                var maxAvailableAmount = _inventoryData.LootItemsHolder[lootFullName].SlotAmount * _maxAmountInSlot[lootFullName];
                var newAmount = _inventoryData.LootItemsHolder[lootFullName].Amount + data.Amount;
                var extra = newAmount - maxAvailableAmount;

                if(extra < 0 )
                {
                    _inventoryData.LootItemsHolder[lootFullName].Amount += data.Amount;
                }
                else if(extra > 0 && _inventoryData.backpackEmptySlotAmount > 0)
                {
                    _inventoryData.backpackEmptySlotAmount -= 1;
                    _inventoryData.LootItemsHolder[lootFullName].SlotAmount += 1;
                    _inventoryData.LootItemsHolder[lootFullName].Amount += data.Amount;
                }
                else
                {
                    _inventoryData.LootItemsHolder[lootFullName].Amount += data.Amount - extra;
                    //ToDo: drop extra
                }
            }
        }
    }
}