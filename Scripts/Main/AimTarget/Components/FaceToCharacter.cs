using Core.MessageBus;
using Core.MessageBus.MessageDataTemplates;
using Main.AimTarget.Data;
using UnityEngine;

namespace Main.AimTarget.Components
{
    public class FaceToCharacter : SubscriberBehaviour
    {
        private AimTargetData _aimTargetData;
        
        protected override void Awake()
        {
            base.Awake();

            _aimTargetData = gameObject.GetComponent<AimTargetData>();
        }

        [Subscribe(SubscribeType.Channel, Characters.API.Messages.UPDATE_POSITION)]           
        public void FollowCursor(Message msg)
        {
            transform.LookAt(((Vector3Data)msg.Data).Value);
            transform.localEulerAngles = new Vector3(0, transform.localEulerAngles.y, transform.localEulerAngles.z);
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