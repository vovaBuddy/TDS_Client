using Core.MessageBus;
using Core.MessageBus.MessageDataTemplates;
using Core.Services;
using Main.Network;
using UnityEngine;
using UnityEngine.Networking;
using System.Collections.Generic;

namespace Main.Characters
{
    public class CharacterFactory : SubscriberBehaviour
    {
        public List<Transform> places;
            
        [Subscribe(SubscribeType.Broadcast, Network.API.Messages.CREATE_CHARACTER)]
        private void CreateCharacter(Message msg)
        {
            var netId = ((IntData)msg.Data).Value;
            var channelId = Channel.GetChannelId();

            ServiceLocator.GetService<ResourceLoaderService>()
                .InstantiatePrefabByPathName("Character/Character",
                    go => {
                        var sub = go.GetComponent<SubscriberBehaviour>();
                        var channel = go.GetComponent<Channel>();
                        //go.transform.position = new Vector3(1198, 114, 1052);
                        //go.transform.position = new Vector3(0, 12, 0);
                        go.transform.position = places[Random.Range(0, places.Count)].position;
                        channel.ChannelIds.Add(SubscribeType.Network, netId);
                        channel.ChannelIds.Add(SubscribeType.Channel, channelId);
                        Channel.ChannelIdByNetId.Add(netId, channelId);
                        sub.ReSubscribe();

                        ServiceLocator.GetService<ResourceLoaderService>()
                            .InstantiatePrefabByPathName("Camera/PlayerCamera",
                                cam => {
                                    var camera = cam.GetComponent<GameCamera.CameraController>();
                                    camera.TargetToFollow = go.transform.Find("CameraPosition");
                                });
                    });
        }

        [Subscribe(SubscribeType.Broadcast, Network.API.Messages.CREATE_CHARACTER_REPLICANT)]
        private void CreateCharacterReplicant(Message msg)
        {
            var netId = ((IntData)msg.Data).Value;
            var channelId = Channel.GetChannelId();

            ServiceLocator.GetService<ResourceLoaderService>()
                .InstantiatePrefabByPathName("Character/Replicant",
                    go => {
                        var sub = go.GetComponent<SubscriberBehaviour>();
                        var channel = go.GetComponent<Channel>();
                        channel.ChannelIds.Add(SubscribeType.Network, netId);
                        channel.ChannelIds.Add(SubscribeType.Channel, channelId);
                        Channel.ChannelIdByNetId.Add(netId, channelId);
                        sub.ReSubscribe();
                    });
        }
    }
}