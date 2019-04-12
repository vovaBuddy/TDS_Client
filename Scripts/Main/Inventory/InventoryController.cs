using UnityEngine;
using System.Collections;
using Core.MessageBus;

namespace Main.Inventory
{
    public class InventoryController : SubscriberBehaviour
    {
        public GameObject InventoryPanel;
        public GameObject BackPackContainer;

        [Subscribe(API.Messages.OPEN_LOOT_MENU, API.Messages.CLOSE_LOOT_MENU)]
        private void ToggleMenu(Message msg)
        {
            InventoryPanel.SetActive(!InventoryPanel.activeSelf);
        }
    }
}