using System;
using Core.MessageBus;
using Core.Services;
using UnityEngine;

namespace Main.Weapons.API
{
    public static class Messages
    {
        public const string INSTANTIATE_L1 = "Main.Weapons.INSTANTIATE_L1";
        public const string INSTANTIATE_L2 = "Main.Weapons.INSTANTIATE_L2";
        public const string INSTANTIATED = "Main.Weapons.INSTANTIATED";
        public const string SHOOT = "Main.Weapons.SHOOT";
        public const string END_SHOOT = "Main.Weapons.END_SHOOT";
        public const string SHOOTING = "Main.Weapons.SHOOT";
        public const string FIRING = "Main.Weapons.FIRING";
        public const string HIT = "Main.Weapons.HIT";

        public const string CHANGE_MOD = "Main.Weapons.CHANGE_MOD";
        public const string START_RELOAD = "Main.Weapons.START_RELOAD";
        public const string END_RELOAD = "Main.Weapons.END_RELOAD";
        public const string UPDATE_MAGAZINE_BULLETS_AMOUNT = "Main.Weapons.UPDATE_MAGAZINE_BULLETS_AMOUNT";
        public const string NO_BULLETS_IN_MAGAZINE = "Main.Weapons.NO_BULLETS_IN_MAGAZINE";
    }
    
    public enum WeaponType
    {
        HEMLOK = 0, 
        WINGMAN = 1,
        PEACEKEEPER = 2,
        PISTOL = 3,
    }

    [Serializable]
    public class WeaponInstantiateData : MessageData
    {
        private static ObjectPool<WeaponInstantiateData> _pool = new ObjectPool<WeaponInstantiateData>(2);

        public static WeaponInstantiateData GetWeaponInstantiateData
            (WeaponType type, Transform parent, int channelId, int ownerNetId, bool createAimTarget)
        {
            var data = _pool.Get();
            data.Type = type;
            data.Parent = parent;
            data.ChannelId = channelId;
            data.CreateAimTarget = createAimTarget;
            data.OwnerNetId = ownerNetId;
            return data;
        }

        public WeaponType Type;
        [NonSerialized]
        public Transform Parent;
        [NonSerialized]
        public int ChannelId;
        [NonSerialized]
        public int OwnerNetId;
        public bool CreateAimTarget;

        public WeaponInstantiateData() { }

        public override void FreeObjectInPool()
        {
            _pool.Release(this);
        }

        public override MessageData GetCopy()
        {
            var data = _pool.Get();
            data.Type = Type;
            data.Parent = Parent;
            data.ChannelId = ChannelId;
            data.CreateAimTarget = CreateAimTarget;
            data.OwnerNetId = OwnerNetId;
            return data;
        }
    }
}