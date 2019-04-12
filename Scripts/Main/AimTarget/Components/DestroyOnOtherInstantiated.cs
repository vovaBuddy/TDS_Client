using Core.MessageBus;

namespace Main.AimTarget.Components
{
    public class DestroyOnOtherInstantiated : SubscriberBehaviour
    {
        [Subscribe(SubscribeType.Channel, API.Messages.DESTROY_OLD_INSTANCE)]
        private void DestroyOnMsg(Message msg)
        {
            Destroy(gameObject);
        }
    }
}

