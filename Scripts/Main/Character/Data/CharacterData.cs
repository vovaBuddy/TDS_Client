using Core.MessageBus;
using Core.MessageBus.MessageDataTemplates;
using Main.Characters.API;
using Main.Characters.Configs;
using UnityEngine;

namespace Main.Characters.Data
{
    public class CharacterData : SubscriberBehaviour, ITransformData
    {
        [HideInInspector] public SpeedMode CurrentSpeedMode { get; set; }

        [SerializeField] private MovementConfig _movementConfig;
        public MovementConfig MovementConfig { get { return _movementConfig; } }

        public Transform RightArm;
        
        public int _hitPoints;

        public int HitPoints
        {
            set
            {
                _hitPoints =  value;
                _hitPoints = Mathf.Min(100, _hitPoints);
                _hitPoints = Mathf.Max(0, _hitPoints);

                Debug.Log("update hp:" + _hitPoints);

                //MessageBus.SendMessage(SubscribeType.Channel, Channel.ChannelIds[SubscribeType.Channel],
                //    CommonMessage.Get( Messages.UPDATE_HIT_POINTS, IntData.GetIntData(_hitPoints)));

                var chan = GetComponent<Channel>();

                MessageBus.SendMessage(NetBroadcastMessage.Get( Network.API.Messages.UPDATE_HIT_POINTS, UnityEngine.Networking.QosType.Reliable,
                    chan.ChannelIds[SubscribeType.Network], IntData.GetIntData(_hitPoints)));

                if (_hitPoints <= 0)
                {
                    Destroy(gameObject);
                }
            }
            get { return _hitPoints; }
        }

        override protected void Awake()
        {
            _hitPoints = 100;
            CurrentSpeedMode = SpeedMode.RUN;
        }

        [Subscribe(SubscribeType.Network, Network.API.Messages.UPDATE_HIT_POINTS)]
        private void UpdateHp(Message msg)
        {
            Debug.Log("Update network hit points!");

            _hitPoints = ((IntData)msg.Data).Value;

            var chan = GetComponent<Channel>();

            MessageBus.SendMessage(SubscribeType.Channel, chan.ChannelIds[SubscribeType.Channel],
                CommonMessage.Get(Messages.UPDATE_HIT_POINTS, IntData.GetIntData(_hitPoints)));


            if (_hitPoints <= 0)
            {
                Destroy(gameObject);
            }
        }

        [Subscribe(SubscribeType.Network, Network.API.Messages.CREATE_WEAPON)]
        private void NeedArmPosition(Message msg)
        {
            var data = (Weapons.API.WeaponInstantiateData)(msg.Data).GetCopy();
            data.OwnerNetId =  gameObject.GetComponent<Channel>().ChannelIds[SubscribeType.Network];
            data.Parent = RightArm;
            MessageBus.SendMessage(CommonMessage.Get(Weapons.API.Messages.INSTANTIATE_L2, data));
        }
    }
}