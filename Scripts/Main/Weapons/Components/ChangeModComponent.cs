using UnityEngine;
using System.Collections.Generic;
using Core.MessageBus;

namespace Main.Weapons.Components
{
    public class ChangeModComponent : SubscriberBehaviour
    {
        private bool mod;
        private SingleNoKDShootingComponent _singleMod;
        private BurstShootingComponent _burstMod;

        protected override void Awake()
        {
            base.Awake();

            _singleMod = gameObject.GetComponent<SingleNoKDShootingComponent>();
            _burstMod = gameObject.GetComponent<BurstShootingComponent>();

            _singleMod.enabled = false;
            _burstMod.enabled = true;
            mod = false;
        }

        [Subscribe(SubscribeType.Channel, API.Messages.CHANGE_MOD)]
        private void ChangeMode(Message msg)
        {
            mod = !mod;
            _singleMod.enabled = mod;
            _burstMod.enabled = !mod;
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