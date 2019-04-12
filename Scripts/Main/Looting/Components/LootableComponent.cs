using Core.MessageBus;
using Main.Inventory.Data;
using Main.Looting.Data;
using UnityEngine;

namespace Main.Looting.Components
{
    public class LootableComponent : SubscriberBehaviour
    {
        protected override void Awake()
        {
            base.Awake();
        }

        public void OnTriggerEnter(Collider other)
        {

        }

        public void OnTriggerExit(Collider other)
        {
        }

        [Subscribe(SubscribeType.Network, Network.API.Messages.DELETE_OBJECT)]
        public void DeleteObject(Message msg)
        {
            Destroy(gameObject);
        }
    }
}