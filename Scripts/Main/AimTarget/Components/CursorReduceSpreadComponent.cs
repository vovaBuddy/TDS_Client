using Core.MessageBus;
using Core.MessageBus.MessageDataTemplates;
using Main.AimTarget.Data;
using Main.Weapons.Configs;
using UnityEngine;

namespace Main.AimTarget.Components
{
    public class CursorReduceSpreadComponent : SubscriberBehaviour
    {
        private AimTargetData _aimTargetData;
        private SpreadConfig _spreadConfig;

        private float _startFireDist = 0.0f;
        private float _curDist = 0;
        private float _delta = 0;
        
        protected override void Awake()
        {
            base.Awake();

            _aimTargetData = gameObject.GetComponent<AimTargetData>();
        }
        
        [Subscribe(SubscribeType.Channel, API.Messages.UPDATE_SPREAD_CONFIG)]
        private void InitSpread(Message msg)
        {
            _spreadConfig = ObjectData.ParseObjectData<SpreadConfig>(msg.Data);
        }

        [Subscribe(SubscribeType.Channel, Characters.API.Messages.UPDATE_POSITION)]           
        public void ReduceCursorSpread(Message msg)
        {
            if(_spreadConfig == null) return;

            var ownerPosition = ((Vector3Data)msg.Data).Value;
            
            _curDist = Vector3.Distance(transform.position, ownerPosition);
            _delta = _startFireDist - _curDist;

            if (_curDist < _startFireDist)
            {
                _aimTargetData.ResultTargetObject.position = Vector3.Lerp(
                    _aimTargetData.ResultTargetObject.position,
                    new Vector3(
                        _aimTargetData.ReduceSpreadTarget.position.x,
                        _aimTargetData.ReduceSpreadTarget.position.y,
                        _aimTargetData.ReduceSpreadTarget.position.z),
                    Time.deltaTime * Mathf.Min(_delta, _spreadConfig.MaxDelta) * _spreadConfig.CursoreReduceSpeed);
            }

            _startFireDist = _curDist;
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