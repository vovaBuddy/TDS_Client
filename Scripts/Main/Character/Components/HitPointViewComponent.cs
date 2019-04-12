using Core.MessageBus;
using Core.MessageBus.MessageDataTemplates;
using UnityEngine;

namespace Main.Characters.Components
{
    public class HitPointViewComponent : SubscriberBehaviour
    {
        public TextMesh HitPointText;

        [Subscribe(SubscribeType.Channel, API.Messages.UPDATE_HIT_POINTS)]
        private void UpdateHP(Message msg)
        {
            HitPointText.text = ((IntData) msg.Data).Value.ToString();
        }
    }
}