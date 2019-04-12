using Core.MessageBus;
using UnityEngine;

namespace Main.AimTarget.Components
{
    public class HideComponent : SubscriberBehaviour
    {
        [Subscribe(SubscribeType.Channel, API.Messages.HIDE)]
        private void Hide(Message msg)
        {
            gameObject.SetActive(false);
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