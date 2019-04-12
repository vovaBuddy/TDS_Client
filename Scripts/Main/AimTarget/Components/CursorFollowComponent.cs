using Core.MessageBus;
using Core.MessageBus.MessageDataTemplates;
using UnityEngine;
using Main.AimTarget.Data;

namespace Main.AimTarget.Components
{
    public class CursorFollowComponent : SubscriberBehaviour
    {
        private AimTargetData _aimTargetData;
        
        private Ray _ray;
        private RaycastHit _hit;

        private float _speed = 100.0f;
        
        protected override void Awake()
        {
            base.Awake();

            _aimTargetData = gameObject.GetComponent<AimTargetData>();
        }

        [Subscribe(SubscribeType.Channel, API.Messages.UPDATE_CURSORE_POSITION)]           
        public void FollowCursor(Message msg)
        {
            var cursorPos = (msg.Data as Vector3Data).Value;
            
            _ray = Camera.main.ScreenPointToRay(cursorPos);

            if (Physics.Raycast(_ray, out _hit, 2000f, _aimTargetData.ExcludeAimLayers))
            {
                transform.position = Vector3.MoveTowards(transform.position, 
                    new Vector3(_hit.point.x, _hit.point.y - 1.32f, _hit.point.z), Time.deltaTime * _speed);
            }
            
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