using Core.MessageBus;
using Core.MessageBus.MessageDataTemplates;
using UnityEngine;
using Main.AimTarget.Data;

namespace Main.AimTarget.Components
{
    public class StartComponent : SubscriberBehaviour
    {
        //protected override void Awake()
        //{
        //    base.Awake();
        //}

        private void Start()
        {
            Debug.Log("Aim target start component");

            MessageBus.SendMessage(SubscribeType.Channel, GetComponent<Channel>().ChannelIds[SubscribeType.Channel],
                CommonMessage.Get(API.Messages.INSTANTIATED, ObjectData.GetObjectData(gameObject)));
        }
    }
}