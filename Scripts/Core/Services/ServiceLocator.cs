using System.Collections.Generic;
using UnityEngine;

namespace Core.Services
{
    public static class ServiceLocator
    {
        private static Dictionary<object, object> _services = null;
        
        public static T GetService<T>(bool createObjectIfNotFound = true) where T : Object
        {
            if (_services == null)
                _services = new Dictionary<object, object>();

            try
            {
                if (_services.ContainsKey(typeof(T)))
                {
                    var service = (T)_services[typeof(T)];
                    if (service != null)  
                    {
                        return service;
                    }
                    else                  
                    {
                        _services.Remove(typeof(T));           
                        return FindService<T>(createObjectIfNotFound);        
                    }
                }
                else
                {
                    return FindService<T>(createObjectIfNotFound);
                }
            }
            catch (System.Exception ex)
            {
                throw new System.NotImplementedException("Can't find requested service, and create new one is set to " 
                                                         + createObjectIfNotFound.ToString());
            }
        }
        
        private static T FindService<T>(bool createObjectIfNotFound = true) where T : Object
        {
            var type = GameObject.FindObjectOfType<T>();
            if (type != null)
            {
                _services.Add(typeof(T), type);
            }
            else if (createObjectIfNotFound)
            {
                var go = new GameObject(typeof(T).Name, typeof(T));
                _services.Add(typeof(T), go.GetComponent<T>());
            }
            return (T)_services[typeof(T)];
        }
    }
}