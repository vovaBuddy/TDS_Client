using Core.MessageBus;
using Core.MessageBus.MessageDataTemplates;
using Main.Characters.Data;
using UnityEngine;

namespace Main.Characters.Components
{
    public class AnimationComponent : SubscriberBehaviour
    {
        private Animator _animator;
        private ITransformData _transformData;

        private float[] _speedMode = { 0.0f, 0.35f, 1.0f };
        private float _curSpeed;
        private bool _in_flight = false;

        public bool Log = false;

        const string ROLL_ACTION = "Roll";
        const string PICKUP_ACTION = "Pickup";
        const string ATTACK_MELEE = "AttackMelee";
        const string MELEE_TYPE = "MeleeType";
        const string FIRE = "Shoot";
        const string AIMING = "Aiming";
        const string RELOAD = "Reloading";
        const string ARMED = "Armed";
        const string JUMP = "Jump";

        protected override void Awake()
        {
            base.Awake();
            
            _animator = GetComponent<Animator>();
            _transformData = gameObject.GetComponent<ITransformData>();
        }

        [Subscribe(SubscribeType.Channel, API.Messages.IN_FLIGHT_START)]
        private void FlightStart(Message msg)
        {
            _in_flight = true;
            //_animator.SetBool(JUMP, true);
        }

        [Subscribe(SubscribeType.Channel, API.Messages.IN_FLIGHT_END)]
        private void FlightEnd(Message msg)
        {
            _in_flight = false;
            //_animator.SetBool(JUMP, false);
        }

        [Subscribe(SubscribeType.Channel, API.Messages.MOVE)]
        private void Move(Message msg)
        {
            if (_in_flight)
            {
                _animator.SetBool("OnGround", false);
                _animator.SetFloat("Y", 0, 0.1f, Time.deltaTime);
                _animator.SetFloat("X", 0, 0.1f, Time.deltaTime);
                return;
            }

            var move = (msg.Data as Vector3Data).Value;
            
            move = move.normalized;

            //Rotate move in camera space
            //            move = Quaternion.Euler(0, 0 - transform.eulerAngles.y +
            //                Camera.main.transform.parent.transform.eulerAngles.y, 0) * move;

            if (Log)
                Debug.Log("Replicant move: " + move);
            
            _animator.SetFloat("Y", move.z, 0.1f, Time.deltaTime);
            _animator.SetFloat("X", move.x, 0.1f, Time.deltaTime);

            _curSpeed = Mathf.Lerp(_curSpeed, _speedMode[(int)_transformData.CurrentSpeedMode], Time.deltaTime * 10);
            _animator.SetFloat("Speed", _curSpeed);

            _animator.SetBool("OnGround", true);
        }

        [Subscribe(SubscribeType.Channel, Weapons.API.Messages.FIRING)]
        private void Firing(Message msg)
        {
            _animator.SetTrigger(FIRE);
        }
        
        [Subscribe(SubscribeType.Channel, Characters.API.Messages.SET_ARMED)]
        private void Armed(Message msg)
        {
            _animator.SetBool(ARMED, true);
        }
        
        [Subscribe(SubscribeType.Channel, Characters.API.Messages.SET_UNARMED)]
        private void Unarmed(Message msg)
        {
            _animator.SetBool(ARMED, false);
        }

        [Subscribe(SubscribeType.Channel, Weapons.API.Messages.START_RELOAD)]
        private void Reload(Message msg)
        {
            //_animator.SetBool(RELOAD, true);
        }
        
        [Subscribe(SubscribeType.Channel, Weapons.API.Messages.END_RELOAD)]
        private void EndReload(Message msg)
        {
            _animator.SetBool(RELOAD, false);
        }

        [Subscribe(SubscribeType.Channel, Characters.API.Messages.START_AIMING)]
        private void StartAiming(Message msg)
        {
            _animator.SetBool(AIMING, true);
        }
        
        [Subscribe(SubscribeType.Channel, Characters.API.Messages.STOP_AIMING)]
        private void StopAiming(Message msg)
        {
            _animator.SetBool(AIMING, false);
        }
    }
}