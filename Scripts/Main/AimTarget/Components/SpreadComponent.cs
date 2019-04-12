using Core.MessageBus;
using Core.MessageBus.MessageDataTemplates;
using Main.AimTarget.Data;
using Main.Weapons.Configs;
using Main.Weapons;
using UnityEngine;

namespace Main.AimTarget.Components
{
    public class SpreadComponent : SubscriberBehaviour
    {
        private AimTargetData _aimTargetData;
        private ISpread _spreadSystem;
        private SpreadConfig _spreadConfig;

        private float stayWithSpreadTimer = 0.0f;
        private float curReduceSpreadSpeed = 5.0f;
        private float realToAimDelta = 0.0f;

        protected override void Awake()
        {
            base.Awake();
            
            _aimTargetData = gameObject.GetComponent<AimTargetData>();
        }

        [Subscribe(SubscribeType.Channel, API.Messages.UPDATE_SPREAD_CONFIG)]
        private void InitSpread(Message msg)
        {
            _spreadConfig = ObjectData.ParseObjectData<SpreadConfig>(msg.Data);
            
            _spreadSystem = new StandardSpread();
            _spreadSystem.Init(_spreadConfig);
        }

        private void Update()
        {
            if (!_spreadConfig) return;
            
            _aimTargetData.ResultTargetObject.position = Vector3.MoveTowards(_aimTargetData.ResultTargetObject.position,
                _aimTargetData.AimTargetObject.position, Time.deltaTime * curReduceSpreadSpeed);

            realToAimDelta = Vector3.Distance(_aimTargetData.ResultTargetObject.position, 
                _aimTargetData.AimTargetObject.position);

            if (stayWithSpreadTimer > 0)
                stayWithSpreadTimer -= Time.deltaTime;
            else
                curReduceSpreadSpeed = _spreadConfig.FastReduceSpreadSpeed;
        }

        [Subscribe(SubscribeType.Channel, Weapons.API.Messages.FIRING)]
        private void Firing(Message msg)
        {
            _spreadSystem.Increase(Time.deltaTime, realToAimDelta <= 0.0001f);
            _aimTargetData.ResultTargetObject.position = 
                _aimTargetData.ResultTargetObject.position + _spreadSystem.Get();
            curReduceSpreadSpeed = _spreadConfig.SlowReduceSpreadSpeed;
            stayWithSpreadTimer = _spreadConfig.StayWithSpread;
        }

        [Subscribe(SubscribeType.Channel, Characters.API.Messages.START_AIMING)]
        private void Aiming(Message msg)
        {
            _spreadSystem.SetAimK(StandardSpread.AIM_ON_K);
        }
        
        [Subscribe(SubscribeType.Channel, Characters.API.Messages.STOP_AIMING)]
        private void StopAiming(Message msg)
        {
            _spreadSystem.SetAimK(StandardSpread.AIM_OFF_K);
        }
        
        [Subscribe(SubscribeType.Channel, Characters.API.Messages.MOVE)]
        private void Move(Message msg)
        {
            if(_spreadSystem == null) return;
            
            var axis = (msg.Data as Vector3Data).Value;
            _spreadSystem.SetAimK(axis.magnitude > 0 ? StandardSpread.MOVE_ON_K : StandardSpread.MOVE_OFF_K);
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