using UnityEngine;
using System.Collections;
using Core.MessageBus;
using Core.Services;

namespace Main.AimTarget.API
{
    public static class Messages
    {
        public const string UPDATE_CURSORE_POSITION = "Main.AimTarget.UPDATE_CURSORE_POSITION";
        public const string UPDATE_AIM_TARGET_POSITION = "Main.AimTarget.UPDATE_AIM_TARGET_POSITION";
        public const string UPDATE_RESULT_TARGET_POSITION = "Main.AimTarget.UPDATE_RESULT_TARGET_POSITION";
        public const string UPDATE_SHOOT_POSITION_TRANSFORM = "Main.AimTarget.UPDATE_SHOOT_POSITION_TRANSFORM";
        public const string UPDATE_SPREAD_CONFIG = "Main.AimTarget.UPDATE_SPREAD_CONFIG";
        public const string INSTANTIATE = "Main.AimTarget.INSTANTIATE";
        public const string INSTANTIATED = "Main.AimTarget.INSTANTIATED";
        public const string DESTROY = "Main.AimTarget.DESTROY";
        public const string HIDE = "Main.AimTarget.HIDE";
        public const string DESTROY_OLD_INSTANCE = "Main.AimTarget.DESTROY_OLD_INSTANCE";
    }
}