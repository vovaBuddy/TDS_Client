using Core.MessageBus;

namespace Main.AimTarget.Components
{
    public class DestroyComponent : SubscriberBehaviour
    {
        [Subscribe(SubscribeType.Channel, API.Messages.DESTROY)]
        private void DestroyOnMsg(Message msg)
        {
            Destroy(gameObject);
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