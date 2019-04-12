using Core.MessageBus;
using Core.MessageBus.MessageDataTemplates;
using Main.Weapons.Data;
using UnityEngine;

namespace Main.Weapons.Components
{
    public class IKComponent : SubscriberBehaviour
    {
        private WeaponData _weaponData;
        
        protected override void Awake()
        {
            base.Awake();
            _weaponData = gameObject.GetComponent<WeaponData>();
        }

        [Subscribe(SubscribeType.Channel,AimTarget.API.Messages.UPDATE_RESULT_TARGET_POSITION)]
        private void IK(Message msg)
        {
            var pos = (msg.Data as Vector3Data).Value;
            
            if (_weaponData.AimingIK)
            {
                transform.LookAt(pos, Vector3.up);
            }
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