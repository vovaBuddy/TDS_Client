using Core.MessageBus;
using Core.MessageBus.MessageDataTemplates;
using Main.Characters.Data;
using UnityEngine;

namespace Main.Characters.Components
{
    public class MoveComponent : SubscriberBehaviour
    {
        private ITransformData _transformData;

        private float[] _speedMode;
        private float _curSpeed;

        private CharacterController charController;

        private bool start = false;
        Vector3 moveDirection = Vector3.zero;
        Vector3 prevMoveDirection = Vector3.zero;
        [SerializeField] private float m_GroundCheckDistance;

        bool _isGrounded = true;

        protected override void Awake()
        {
            base.Awake();
        
            _transformData = gameObject.GetComponent<ITransformData>();
            _speedMode = new float[3] {  _transformData.MovementConfig.SideSpeed, 
                _transformData.MovementConfig.RunSpeed,  _transformData.MovementConfig.SprintSpeed};

            _transformData.CurrentSpeedMode = SpeedMode.RUN;
            charController = GetComponent<CharacterController>();
        }

        private bool IsGround()
        {
            RaycastHit hitInfo;

            if (Physics.Raycast(transform.position + (Vector3.up * 0.1f), Vector3.down, out hitInfo, m_GroundCheckDistance))
            {
                return true;
            }

            return false;
        }

        [Subscribe(SubscribeType.Channel, API.Messages.MOVE)]
        private void Move(Message msg)
        {
            if (charController.isGrounded)
                start = true;

            if (!start) return;

            if (_isGrounded && !IsGround())
            {
                MessageBus.SendMessage(SubscribeType.Channel, Channel.ChannelIds[SubscribeType.Channel], 
                    CommonMessage.Get(API.Messages.IN_FLIGHT_START));
                _isGrounded = false;
            }

            if(!_isGrounded && IsGround())
            {
                MessageBus.SendMessage(SubscribeType.Channel, Channel.ChannelIds[SubscribeType.Channel], 
                    CommonMessage.Get(API.Messages.IN_FLIGHT_END));
                _isGrounded = true;
            }


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

            //transform.position = Vector3.Lerp(transform.position, pos, Time.deltaTime * 500);

            if (charController.isGrounded)
            {
                // We are grounded, so recalculate
                // move direction directly from axes

                moveDirection = new Vector3(
                    axis.x * sign * _transformData.MovementConfig.SideSpeed,
                    0.0f,
                    axis.z * (axis.z > 0 ? _curSpeed : _transformData.MovementConfig.SideSpeed));

                moveDirection = transform.TransformDirection(moveDirection);

                if (Input.GetKeyDown(KeyCode.Space))
                {
                    moveDirection.y = 9.0f;
                }
            }

            // Apply gravity
            //if(moveDirection.y > 0)
                moveDirection.y = moveDirection.y - (20 * Time.deltaTime);

            // Move the controller
            charController.Move(moveDirection * Time.deltaTime);

            MessageBus.SendMessage(SubscribeType.Channel, Channel.ChannelIds[SubscribeType.Channel],
                CommonMessage.Get(API.Messages.UPDATE_POSITION, 
                Vector3Data.GetVector3Data(transform.position)));

            //Debug.Log("is grounded! :" + charController.isGrounded);
            //Debug.Log("my is grounded! :" + IsGround());
        }
    }
}