using Core.MessageBus;
using Core.MessageBus.MessageDataTemplates;
using Main.Characters.Data;
using UnityEngine;

namespace Main.Characters.Components
{
    public class ReplicantMoveComponent : SubscriberBehaviour
    {
        private ITransformData _transformData;

        private float[] _speedMode;
        private float _curSpeed;

        protected override void Awake()
        {
            base.Awake();

            _transformData = gameObject.GetComponent<ITransformData>();
            _speedMode = new float[3] {  _transformData.MovementConfig.SideSpeed,
                _transformData.MovementConfig.RunSpeed,  _transformData.MovementConfig.SprintSpeed};

            _transformData.CurrentSpeedMode = SpeedMode.RUN;
        }

        [Subscribe(SubscribeType.Channel, API.Messages.MOVE)]
        private void Move(Message msg)
        {
            var axis = (msg.Data as Vector3Data).Value;

            axis = axis.normalized;

            //Rotate move in camera space
            //            axis = Quaternion.Euler(0, 0 - transform.eulerAngles.y +
            //                Camera.main.transform.parent.transform.eulerAngles.y, 0) * axis;
            //
            //            axis.x *=  _transformData.MovementConfig.SideSpeed;
            _curSpeed = Mathf.Lerp(_curSpeed, _speedMode[(int)_transformData.CurrentSpeedMode], Time.deltaTime * 10);
            //            axis.z *= axis.z > 0 ? _curSpeed :  _transformData.MovementConfig.SideSpeed;
            //
            //            transform.Translate(axis * Time.deltaTime);

            var pos = transform.position;

            pos = pos +
                transform.forward * axis.z * Time.deltaTime *
                    (axis.z > 0 ? _curSpeed : _transformData.MovementConfig.SideSpeed);

            var sign = Vector3.Angle(transform.forward, Camera.main.gameObject.transform.forward) > 80 ? -1 : 1;

            pos = pos +
                transform.right * axis.x * sign * Time.deltaTime * _transformData.MovementConfig.SideSpeed;

            transform.position = Vector3.Lerp(transform.position, pos, Time.deltaTime * 500);

            MessageBus.SendMessage(SubscribeType.Channel, Channel.ChannelIds[SubscribeType.Channel],
                CommonMessage.Get(API.Messages.UPDATE_POSITION,
                Vector3Data.GetVector3Data(transform.position)));
        }
    }
}