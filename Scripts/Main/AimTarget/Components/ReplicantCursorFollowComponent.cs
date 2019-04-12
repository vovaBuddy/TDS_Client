using UnityEngine;
using System.Collections;
using Main.AimTarget.Data;
using Core.MessageBus;
using Core.MessageBus.MessageDataTemplates;

namespace Main.AimTarget
{
    public class ReplicantCursorFollowComponent : SubscriberBehaviour
    {
        private AimTargetData _aimTargetData;

        protected override void Awake()
        {
            base.Awake();

            _aimTargetData = gameObject.GetComponent<AimTargetData>();
        }

        public void Update()
        {
            MessageBus.SendMessage(SubscribeType.Channel, Channel.ChannelIds[SubscribeType.Channel],
                CommonMessage.Get(API.Messages.UPDATE_AIM_TARGET_POSITION,
                    Vector3Data.GetVector3Data(_aimTargetData.AimTargetObject.position)));
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
