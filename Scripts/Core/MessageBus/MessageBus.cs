using System;
using UnityEngine;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using Main.Network;

namespace Core.MessageBus
{
    public delegate void SubscriberAction(Message msg);
    
    public class MessageBus : MonoBehaviour
    {
        private MessageQueue _queue;
        private List<Message> _waitingList;
        private Message _procMsg;
        private Dictionary<string, List<SubscriberAction>> _subscribers;

        private static MessageBus _instance;
        public static MessageBus Instance
        {
            get {
                if (_instance == null)
                {
                    _instance = GameObject.FindObjectOfType<MessageBus>();
             
                    if (_instance == null)
                    {
                        GameObject container = new GameObject("__MESSAGE_BUS");
                        _instance = container.AddComponent<MessageBus>();
                    }
                }
     
                return _instance;
            }
        }

        private void _AddSubscriber(string type, object go, MethodInfo methodInfo)
        {
            if (!_subscribers.ContainsKey(type))
            {
                _subscribers.Add(type, new List<SubscriberAction>());
            }
            
            SubscriberAction method = 
                (SubscriberAction)Delegate.CreateDelegate(typeof(SubscriberAction), go, methodInfo.Name);   
            
            _subscribers[type].Add(method);
        }
        
        public void AddSubscriber(SubscribeType sType, string type, object go, MethodInfo methodInfo)
        {
            var channelIds = (go as MonoBehaviour).GetComponent<Channel>().ChannelIds;

            if(channelIds.ContainsKey(sType) && sType != SubscribeType.Broadcast)
            {
                _AddSubscriber(sType.ToString("d") + channelIds[sType] + "_" + type, go, methodInfo);
            }
            else if(sType == SubscribeType.Broadcast)
            {
                _AddSubscriber(type, go, methodInfo);
            }
            
        }

        private void _Unsubscribe(string type, object go, MethodInfo methodInfo)
        {
            //ToDo: change List to Dictionary
            if (_subscribers.ContainsKey(type))
            {
                for (var i = 0; i < _subscribers[type].Count; ++i)
                {
                    if (_subscribers[type][i].Method == methodInfo &&
                        _subscribers[type][i].Target == go)
                    {
                        _subscribers[type].RemoveAt(i);
                        return;
                    }
                }

                if(_subscribers[type].Count == 0)
                    _subscribers.Remove(type);
            } 
        }

        public void Unsubscribe(SubscribeType sType, string type, object go, MethodInfo methodInfo)
        {
            var channelIds = (go as MonoBehaviour).GetComponent<Channel>().ChannelIds;
            if (channelIds.ContainsKey(sType) && sType != SubscribeType.Broadcast)
            {
                _Unsubscribe(sType.ToString("d") + channelIds[sType] + "_" + type, go, methodInfo);
            }
            else if(sType == SubscribeType.Broadcast)
            {
                _Unsubscribe(type, go, methodInfo);
            }
        }

        public static void SendMessage(Message msg, bool waitingForSubscriber = false)
        {
            msg.WaitingForSubscriber = waitingForSubscriber;
            MessageBus.Instance._queue.Push(msg);
        }
        
        public static void SendMessage(SubscribeType sType, int id, 
            Message msg, bool waitingForSubscriber = false)
        {

            if (sType != SubscribeType.Broadcast && !msg.GeneratedChannelMsgType)
            {
                msg.Type = sType.ToString("d") + id + "_" + msg.Type;
                msg.GeneratedChannelMsgType = true;
            }

            msg.WaitingForSubscriber = waitingForSubscriber;
            MessageBus.Instance._queue.Push(msg);
        }
        
        private void Awake()
        {
            _queue = new MessageQueue(1000);
            _subscribers = new Dictionary<string, List<SubscriberAction>>();
            
            _waitingList = new List<Message>();
        }
        
        //ToDo: check for fps 
        private void LateUpdate()
        {
            for (var i = _waitingList.Count - 1; i >= 0; --i)
            {
                if (_subscribers.ContainsKey(_waitingList[i].Type))
                {
                    _queue.Push(_waitingList[i]);
                    _waitingList.RemoveAt(i);
                }
            }
            
            for(_procMsg = _queue.Pop(); _procMsg != null; _procMsg = _queue.Pop())
            {
                if(_procMsg.Type == null) continue;
                
                //net
                if (_procMsg is ISendMessage)
                {
                    (_procMsg as ISendMessage).SendMessage();
                }
                
                if (_subscribers.ContainsKey(_procMsg.Type) && _subscribers[_procMsg.Type].Count != 0)
                {
                    for (var i = 0; i < _subscribers[_procMsg.Type].Count; ++i)
                    {
                        var _delegate = _subscribers[_procMsg.Type][i];

                        if ((_delegate.Target as SubscriberBehaviour).gameObject.activeSelf)
                        {
                            //_subscribers[_procMsg.Type][i](_procMsg);
                            try
                            {
                                _subscribers[_procMsg.Type][i](_procMsg);
                            }
                            catch (Exception e)
                            {
                                Console.WriteLine(e);
                            }
                        }
                        else
                        {
                            Debug.Log("Object subscribed to" + _procMsg.Type + " - " +
                                    (_delegate.Target as SubscriberBehaviour).gameObject.name + " is disabled");
                        }
                    }
                    
                    if(_procMsg.Data != null)
                        _procMsg.Data.FreeObjectInPool();
                    _procMsg.FreePooledObject();
                }
                else 
                {
                    if (!(_procMsg is ISendMessage))
                        Debug.Log("There are no subscribers for " + _procMsg.Type + "; wait is " + 
                              _procMsg.WaitingForSubscriber.ToString());
                    
                    if (_procMsg.WaitingForSubscriber)
                    {
                        _waitingList.Add(_procMsg);
                    }
                    else
                    {
                        if(_procMsg.Data != null)
                            _procMsg.Data.FreeObjectInPool();
                        _procMsg.FreePooledObject();
                    }
                }
            }
        }
    }
}