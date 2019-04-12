using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using UnityEngine;

namespace Core.MessageBus
{
    public class SubscriberBehaviour : MonoBehaviour
    {
        [HideInInspector]
        public Channel Channel;

        protected const BindingFlags _bindingFlags = 
            BindingFlags.Public | BindingFlags.Static | BindingFlags.NonPublic | 
            BindingFlags.Instance | BindingFlags.FlattenHierarchy;

        public virtual void Subscribe()
        {           
            var methods = this.GetType().GetMethods(_bindingFlags).Where(
                prop => Attribute.IsDefined(prop, typeof(Subscribe)));

            foreach (var m in methods)
            {      
                var attr = (Subscribe)m.GetCustomAttributes(typeof(Subscribe), true)[0];

                for (var i = 0; i < attr.subscribed_types.Length; ++i)
                {
                    MessageBus.Instance.AddSubscriber(attr.Type, attr.subscribed_types[i], this, m);
                }
            }
        }

        public virtual void Unsubscribe()
        {            
            var subMethods = this.GetType().GetMethods(_bindingFlags).Where(
                prop => Attribute.IsDefined(prop, typeof(Subscribe)));

            foreach (var m in subMethods)
            {      
                var attr = (Subscribe)m.GetCustomAttributes(typeof(Subscribe), true)[0];

                for (var i = 0; i < attr.subscribed_types.Length; ++i)
                {
                    MessageBus.Instance.Unsubscribe(attr.Type, attr.subscribed_types[i], this, m);
                }
            }
        }

        public virtual void ReSubscribe()
        {
            foreach (var sub in gameObject.GetComponentsInChildren<SubscriberBehaviour>())
            {
                sub.Unsubscribe();
                sub.Subscribe();
                sub.AfterAction();
            }
        }

        public virtual void AfterAction()
        {
            
        }
        
        protected virtual void Awake()
        {
            Channel = gameObject.GetComponent<Channel>();

            if(Channel == null)
            {
                Channel = gameObject.AddComponent<Channel>();
            }

            Subscribe();
        }

        protected virtual void OnDestroy()
        {
            Unsubscribe();
        }
    }
}