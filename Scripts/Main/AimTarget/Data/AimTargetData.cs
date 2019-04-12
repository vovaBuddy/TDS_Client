using Core.MessageBus;
using Core.MessageBus.MessageDataTemplates;
using UnityEngine;

namespace Main.AimTarget.Data
{
    public class AimTargetData : SubscriberBehaviour
    {
        public Transform AimTargetObject;
        public Transform ReduceSpreadTarget;
        public Transform ResultTargetObject;
        public Transform StickyAimObject;

        public LayerMask ExcludeAimLayers;
        public LayerMask TargetsAimLayers;

        //[HideInInspector]
        public Transform ShootPosition;

        [Subscribe(SubscribeType.Channel, API.Messages.UPDATE_SHOOT_POSITION_TRANSFORM)]
        private void UpdateShootPosition(Message msg)
        {
            Debug.Log("Recieve shoot position " + ObjectData.ParseObjectData<Transform>(msg.Data).name);

            ShootPosition = ObjectData.ParseObjectData<Transform>(msg.Data);

            Debug.Log(gameObject.name);
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