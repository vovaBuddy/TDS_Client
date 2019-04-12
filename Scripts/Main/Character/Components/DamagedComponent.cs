using System.Collections.Generic;
using Core.MessageBus;
using UnityEngine;

namespace Main.Characters.Components
{
    public class DamagedComponent : SubscriberBehaviour
    {
        public List<int> BulletIds;

        protected override void Awake()
        {
            base.Awake();
            
            BulletIds = new List<int>();
        }

//        [Subscribe(SubscribeType.Channel, API.Messages.REGISTER_PROJECTILE)]
//        private void Register(Message msg)
//        {
//            var id = (msg.Data as GenericData<int>).Value;
//            
//            BulletIds.Add(id);
//        }
//
//        [Subscribe(API.Messages.UNREGISTER_PROJECTILE)]
//        private void Unregister(Message msg)
//        {
//            var id = (msg.Data as GenericData<int>).Value;
//
//            var index = BulletIds.IndexOf(id);
//            
//            if(index == -1) return;
//            
//            BulletIds.RemoveAt(index);
//        }
    }
}