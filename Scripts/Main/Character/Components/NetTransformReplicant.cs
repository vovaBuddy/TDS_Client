using UnityEngine;
using System.Collections;
using Core.MessageBus;
using Core.MessageBus.MessageDataTemplates;

namespace Main.Characters
{
    public class NetTransformReplicant : SubscriberBehaviour
    {
        private float timeStartedLerping;
        private float timeToLerp;
        private Vector3 realPosition;
        private Vector3 lastRealPosition;

        private Quaternion realAngles;
        private Quaternion lastRealAngles;

        [Subscribe(SubscribeType.Network, Network.API.Messages.SYNC_TRANSFORM)]
        private void SyncTransform(Message msg)
        {
            var data = ((SyncTransformTimedData)msg.Data);

            lastRealPosition = realPosition;
            realPosition = data.GetVector3();

            timeToLerp = data.Time;

            timeStartedLerping = Time.time;
        }

        private void FixedUpdate()
        {
            var lerpPercentage = (Time.time - timeStartedLerping) / timeToLerp;

            transform.position = Vector3.Lerp(lastRealPosition, realPosition, lerpPercentage);

            //if (Vector3.Distance(realPosition, lastRealPosition) < 0.01f)
            //{
            //    MessageBus.SendMessage(SubscribeType.Channel, Channel.ChannelIds[SubscribeType.Channel],
            //        CommonMessage.Get(API.Messages.MOVE, Vector3Data.GetVector3Data(Vector3.zero)));
            //}
            //else
            //{
                var localRot = transform.localRotation;
                var direction = realPosition - lastRealPosition;
                MessageBus.SendMessage(SubscribeType.Channel, Channel.ChannelIds[SubscribeType.Channel],
                    CommonMessage.Get(API.Messages.MOVE,
                        Vector3Data.GetVector3Data(Quaternion.Inverse(localRot) * direction)));
            //}
        }
    }
}