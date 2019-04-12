using UnityEngine;

namespace Core.Services
{
    public delegate void ResourceLoaderCallback(GameObject go);
    
    public abstract class ResourceLoaderService : MonoBehaviour
    {
        public abstract void InstantiatePrefabByPathName(string pathName, ResourceLoaderCallback callback);
    }
}