using Core.MessageBus;
using Core.MessageBus.MessageDataTemplates;
using UnityEngine;
using AimTargetData = Main.AimTarget.Data.AimTargetData;

namespace Main.AimTarget.Components
{
    public class StickyCursorFollowComponent : SubscriberBehaviour
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
        
        [Subscribe(SubscribeType.Channel,API.Messages.UPDATE_CURSORE_POSITION)]           
        public void FollowCursor(Message msg)
        {
            var cursorPos = (msg.Data as Vector3Data).Value;
            
            _ray = Camera.main.ScreenPointToRay(cursorPos);

            if (Physics.Raycast(_ray, out _hit, 2000f, _aimTargetData.TargetsAimLayers))
            {
                _aimTargetData.AimTargetObject.transform.parent.GetComponent<MeshRenderer>().enabled = false;
                
                transform.position = Vector3.MoveTowards(transform.position, 
                    new Vector3(_hit.point.x, _hit.point.y - 1.32f, _hit.point.z), Time.deltaTime * _speed);
                
                _aimTargetData.StickyAimObject.position = Vector3.MoveTowards(transform.position, 
                    _hit.point, Time.deltaTime * _speed);
            }
            else if (Physics.Raycast(_ray, out _hit, 2000f, _aimTargetData.ExcludeAimLayers))
            {
                _aimTargetData.AimTargetObject.transform.parent.GetComponent<MeshRenderer>().enabled = true;
                
                var finalPos = new Vector3(_hit.point.x, _hit.point.y, _hit.point.z);

                transform.position = Vector3.MoveTowards(transform.position, 
                    finalPos, Time.deltaTime * _speed);
            }
            
            MessageBus.SendMessage(SubscribeType.Channel, Channel.ChannelIds[SubscribeType.Channel],
                CommonMessage.Get(API.Messages.UPDATE_AIM_TARGET_POSITION, 
                Vector3Data.GetVector3Data(_aimTargetData.AimTargetObject.position)));
            
            if(_aimTargetData.ResultTargetObject == null) return;
            
            MessageBus.SendMessage(SubscribeType.Channel, Channel.ChannelIds[SubscribeType.Channel],
                CommonMessage.Get( API.Messages.UPDATE_RESULT_TARGET_POSITION, 
                Vector3Data.GetVector3Data(_aimTargetData.ResultTargetObject.position)));
            
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