using UnityEngine;
using System.Collections;
using Core.MessageBus;
using Core.MessageBus.MessageDataTemplates;

namespace Main.Character
{
    public class NetTransform : SubscriberBehaviour
    {
        public float SendRate = 4.0f;

        private float timeBetweenMovementStart;
        private float timeBetweenMovementEnd;
        private bool canSendNetworkMovement;

        private IEnumerator StartNetworkSendCooldown()
        {
            timeBetweenMovementStart = Time.time;
            yield return new WaitForSeconds((1 / SendRate));
            SendNetworkMovement();
        }

        private void SendNetworkMovement()
        {
            timeBetweenMovementEnd = Time.time;

            MessageBus.SendMessage(NetBroadcastMessage.Get(
                Network.API.Messages.SYNC_TRANSFORM,
                UnityEngine.Networking.QosType.Unreliable, Channel.ChannelIds[SubscribeType.Network],
                SyncTransformTimedData.GetData(
                    transform.position, timeBetweenMovementEnd - timeBetweenMovementStart)));

            canSendNetworkMovement = false;
        }

        private void Update()
        {
            if (!canSendNetworkMovement)
            {
                canSendNetworkMovement = true;
                StartCoroutine(StartNetworkSendCooldown());
            }
        }
    }
}
