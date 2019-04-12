using UnityEngine;
using System.Collections.Generic;
using Core.MessageBus;
using Core.Services;
using Core.MessageBus.MessageDataTemplates;
using Main.Looting.Data;

namespace Main.Inventory
{
    public class InventoryPanel : SubscriberBehaviour
    {
        public GameObject BackPackContainer;
        private int BackPackSlotsAmount = 8;
        private GameObject[] _slots;

        protected override void Awake()
        {
            base.Awake();
            _slots = new GameObject[BackPackSlotsAmount];

            for (int i = 0; i < BackPackSlotsAmount; ++i)
            {
                ServiceLocator.GetService<ResourceLoaderService>().InstantiatePrefabByPathName("GUI/Backpack/BackpackSlot",
                    go =>
                    {
                        go.transform.SetParent(BackPackContainer.transform, false);
                        _slots[i] = go;
                    });
            }
        }

        [Subscribe(API.Messages.OPEN_LOOT_MENU)]
        private void ToggleMenu(Message msg)
        {
            var lootItemHolders = ((ObjectData)msg.Data).Value as Dictionary<string, LootItemHolder>;

            int curSlotIndex = -1;

            if (lootItemHolders.ContainsKey(LootData.GetFullLootName(Bullets.API.BulletType.HEAVY)))
            {
                var itemHolder = lootItemHolders[LootData.GetFullLootName(Bullets.API.BulletType.HEAVY)];

                for (int i = 0; i < itemHolder.SlotAmount; ++i)
                {
                    curSlotIndex++;

                    var slot = _slots[curSlotIndex].GetComponent<BackpackSlot>();
                    slot.ItemIcon.gameObject.SetActive(true);

                    if (i == itemHolder.SlotAmount - 1)
                    {
                        slot.Amount.text = (itemHolder.Amount - (itemHolder.SlotAmount - 1) * itemHolder.MaxInSlot).ToString();
                    }
                    else
                    {
                        slot.Amount.text = itemHolder.MaxInSlot.ToString();
                    }
                }
            }

            if (lootItemHolders.ContainsKey(LootData.GetFullLootName(Bullets.API.BulletType.SHELLS)))
            {
                var itemHolder = lootItemHolders[LootData.GetFullLootName(Bullets.API.BulletType.SHELLS)];

                for (int i = 0; i < itemHolder.SlotAmount; ++i)
                {
                    curSlotIndex++;

                    var slot = _slots[curSlotIndex].GetComponent<BackpackSlot>();
                    slot.ItemIcon.gameObject.SetActive(true);

                    if (i == itemHolder.SlotAmount - 1)
                    {
                        slot.Amount.text = (itemHolder.Amount - (itemHolder.SlotAmount - 1) * itemHolder.MaxInSlot).ToString();
                    }
                    else
                    {
                        slot.Amount.text = itemHolder.MaxInSlot.ToString();
                    }
                }
            }
        }
    }
}