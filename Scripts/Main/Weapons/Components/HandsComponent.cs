using Core.MessageBus;

namespace Main.Weapons.Components
{
    public class HandsComponent : SubscriberBehaviour
    {
        [Subscribe(SubscribeType.Channel,API.Messages.HIT)]
        private void Hit(Message msg)
        {
            
        }
        
    }
}