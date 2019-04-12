using System;
using Core.Services;
using UnityEngine;

namespace Services
{
    public class ResourceLoader : ResourceLoaderService
    {
        public override void InstantiatePrefabByPathName(string pathName, ResourceLoaderCallback callback)
        {
            callback(Instantiate(Resources.Load<GameObject>(pathName)));
        }
    }
}