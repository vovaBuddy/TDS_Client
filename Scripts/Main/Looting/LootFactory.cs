using System;
using System.Collections.Generic;
using Core.MessageBus;
using Core.Services;
using Main.Looting.Data;
using Main.Network;
using Services;
using UnityEngine;

namespace Main.Looting
{
    public class LootFactory : SubscriberBehaviour
    {
        [SerializeField] private GameObject _lootItemsContainer;

        public static LootFactory Instance;

        protected override void Awake()
        {
            base.Awake();

            Instance = this;
        }

        [Subscribe(SubscribeType.Broadcast, Network.API.Messages.CREATE_LOOT_ITEM)]
        private void Spawn(Message msg)
        {
            Debug.Log("Create loot item");

            var lootData = (InstanceLootItemData)msg.Data;

            ServiceLocator.GetService<ResourceLoader>().InstantiatePrefabByPathName(
                "Loot/LootItem_" + lootData.LootType.ToString("d") + String.Join("", lootData.Params),
                go =>
            {
                go.transform.SetParent(_lootItemsContainer.transform);
                go.transform.position = lootData.GetVector3();

                var curLootData = go.GetComponent<LootData>();
                curLootData.LootType = lootData.LootType;

                curLootData.Amount = lootData.Amout;

                curLootData.Params = lootData.Params;

                var sub = go.GetComponent<SubscriberBehaviour>();
                sub.Channel.ChannelIds.Add(SubscribeType.Network, lootData.NetId);
                sub.ReSubscribe();
            });            
        }
    }
}