using Core.MessageBus;
using Core.MessageBus.MessageDataTemplates;
using Main.Characters.Configs;
using Main.Characters.Data;
using Main.AimTarget;
using UnityEngine;

namespace Main.Characters.Components
{
    public class FaceToAimTarget : SubscriberBehaviour
    {        
        private ITransformData _transformData;

        protected override void Awake()
        {
            base.Awake();
        
            _transformData = gameObject.GetComponent<ITransformData>();
        }
        
        [Subscribe(SubscribeType.Channel, AimTarget.API.Messages.UPDATE_AIM_TARGET_POSITION)]
        private void Move(Message msg)
        {
            var aimTargetPos = (msg.Data as Vector3Data).Value;
            
            transform.rotation = Quaternion.Lerp( transform.rotation, 
                Quaternion.LookRotation(aimTargetPos - transform.position),
                Time.deltaTime *  _transformData.MovementConfig.RotateSpeed);
            transform.localEulerAngles = new Vector3(0, transform.localEulerAngles.y, 0);
        }
    }
}