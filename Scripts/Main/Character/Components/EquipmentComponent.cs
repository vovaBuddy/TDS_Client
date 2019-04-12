using Core.MessageBus;
using Core.MessageBus.MessageDataTemplates;
using Main.Inventory.Data;
using Main.Characters.Data;

namespace Main.Characters.Components
{
    public class EquipmentComponent : SubscriberBehaviour
    {
        private InventoryData _data;
        private CharacterData _characterData;
        
        protected override void Awake()
        {
            base.Awake();

            _data = GetComponentInChildren<InventoryData>();
            _characterData = GetComponentInChildren<CharacterData>();
        }

        [Subscribe(SubscribeType.Channel, API.Messages.START_HEALING)]
        private void Heal(Message msg)
        {
            
        }
        
        [Subscribe(SubscribeType.Channel, API.Messages.UPDATE_HIT_POINTS)]
        private void UpdateHP(Message msg)
        {
            var hp = ((IntData) msg.Data).Value;

            _characterData._hitPoints = hp;
            
            if (hp <= 0)
            {
                MessageBus.SendMessage(SubscribeType.Channel, Channel.ChannelIds[SubscribeType.Channel],
                    CommonMessage.Get(API.Messages.DEAD));    
            }
        }
    }
}