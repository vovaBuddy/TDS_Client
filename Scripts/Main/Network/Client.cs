using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;
using Core.MessageBus;
using Core.MessageBus.MessageDataTemplates;
using Main.Network.API;
using UnityEngine;
using UnityEngine.Networking;

namespace Main.Network
{
    public class Client : SubscriberBehaviour
    {
        private const int MAX_USER = 100;
        private const int PORT = 26000;
        //private const string SERVER_IP = "159.69.45.209";
        private const string SERVER_IP = "192.168.0.3";
        //private const string SERVER_IP = "127.0.0.1";
        
        private Dictionary<QosType, byte> _QoSChannels = new Dictionary<QosType, byte>();
		
        private int _connectionId;
        private int _hostId;
        private byte _error;

        private int BUFFER_LENGTH = 1024;

        private bool _isStarted;

        public static Client Instance;
        
        protected override void Awake()
        {
            base.Awake();
            
            Instance = this;
            
            NetworkTransport.Init();
			
            var config = new ConnectionConfig();
            _QoSChannels.Add(QosType.Unreliable, config.AddChannel(QosType.Unreliable));
            _QoSChannels.Add(QosType.Reliable, config.AddChannel(QosType.Reliable));
            _QoSChannels.Add(QosType.ReliableSequenced, config.AddChannel(QosType.ReliableSequenced));

            var topology = new HostTopology(config, MAX_USER);

            _hostId = NetworkTransport.AddHost(topology, 0);

            _connectionId = NetworkTransport.Connect(_hostId, SERVER_IP, PORT, 0, out _error);
            
            _isStarted = true;
            
            StartCoroutine(SayHello());
        }
        
        public void SendMessage(NetMessage netMsg, QosType qos)
        {
            var buffer = new byte[BUFFER_LENGTH];
            var formatter = new BinaryFormatter();
            var stream = new MemoryStream(buffer);

            
            formatter.Serialize(stream, netMsg);
			
            NetworkTransport.Send(_hostId, _connectionId, _QoSChannels[qos], 
                buffer, BUFFER_LENGTH, out _error);	
            
            stream.Close();
        }

        private IEnumerator SayHello()
        {
            yield return new WaitForSeconds(1);
            
            MessageBus.SendMessage(NetServerMessage.Get(API.Messages.READY_FOR_CHARACTER, QosType.Reliable));
        }
        
        private void Update()
        {
            ReceiveMessage();
        }

        private void ReceiveMessage()
        {
            int recHostId; 
            int connectionId; 
            int channelId; 
            byte[] recBuffer = new byte[BUFFER_LENGTH]; 
            int dataSize;
            byte error;
			
            while(true)
            {
                NetworkEventType recData = NetworkTransport.Receive(out recHostId, out connectionId, out channelId,
                    recBuffer, BUFFER_LENGTH, out dataSize, out error);
                switch (recData)
                {
                    case NetworkEventType.ConnectEvent:
                        Debug.Log(string.Format("Client connected hostId:{0}, connectionId:{1}", recHostId,
                            connectionId));
                        break;

                    case NetworkEventType.DataEvent:
                        var formatter = new BinaryFormatter();
                        var stream = new MemoryStream(recBuffer);
                        //ParseData(connectionId, formatter.Deserialize(stream) as NetworkMessage);

                        var netMsg = (NetMessage)formatter.Deserialize(stream);
                        
                        //Debug.Log("Recieve: " + netMsg.Type);

                        MessageBus.SendMessage(CommonMessage.Get(
                            (netMsg.NetId == -1 ? netMsg.Type.ToString() :
                                Channel.GetFullSubscribeType(
                                    SubscribeType.Network, netMsg.NetId, netMsg.Type.ToString())),
                            (MessageData)netMsg.Data));

                        stream.Close();
                        
                        break;
                    case NetworkEventType.DisconnectEvent: break;

                    case NetworkEventType.BroadcastEvent: break;
                    default:
                    case NetworkEventType.Nothing: 
                        return;
                        break;
                }
            }
        }
    }
}