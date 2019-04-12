using UnityEngine;
using System.Collections;
using Core.MessageBus;
using Main.Network;
using UnityEngine.Networking;
using Core.Services;

public class NetBroadcastMessage : Message, ISendMessage
{
    public byte NetType;
    public int NetId;
    public QosType QoS;

    public void SendMessage()
    {
        NetMessage netMsg;
        netMsg.Type = NetType;
        netMsg.Data = Data;
        netMsg.NetId = NetId;
        netMsg.Broadcast = true;

        Client.Instance.SendMessage(netMsg, QoS);
    }

    private static ObjectPool<NetBroadcastMessage> _pool
        = new ObjectPool<NetBroadcastMessage>(10);

    public NetBroadcastMessage() : base()
    {
        NetId = -1;
        NetType = 0;
        QoS = QosType.AllCostDelivery;
    }

    public static NetBroadcastMessage Get(byte msgType, QosType qos, int netId = -1, MessageData data = null)
    {
        var msg = _pool.Get();
        msg.Type = netId == -1 ? msgType.ToString() : 
            Channel.GetFullSubscribeType(SubscribeType.Network, netId, msgType.ToString());
        msg.NetType = msgType;
        msg.NetId = netId;
        msg.QoS = qos;
        msg.Data = data;
        return msg;
    }

    public override void FreePooledObject()
    {
        NetId = -1;
        NetType = 0;
        Data = null;
        Type = null;
        QoS = QosType.AllCostDelivery;

        _pool.Release(this);
    }
}
