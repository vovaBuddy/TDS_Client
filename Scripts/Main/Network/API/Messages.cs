using Core.MessageBus;

namespace Main.Network.API
{
    public static class Messages
    {
        public const byte READY_FOR_CHARACTER = 1;
        public const byte CREATE_CHARACTER = 2;
        public const byte CREATE_AIM_TARGET = 3;
        public const byte CREATE_LOOT_ITEM = 4;
        public const byte TRY_PICK_UP_NEAR = 5;
        public const byte PICK_UP_LOOT_ITEM = 6;
        public const byte ADD_NEAR_OBJECT = 7;
        public const byte REMOVE_NEAR_OBJECT = 8;

        public const byte SYNC_TRANSFORM = 9;
        public const byte SYNC_STATIC = 10;
        public const byte DELETE_OBJECT = 11;

        public const byte CREATE_CHARACTER_REPLICANT = 12;
        public const byte CREATE_AIM_TARGET_REPLICANT = 13;

        public const byte CREATE_WEAPON = 14;
        public const byte CREATE_PROJECTILE = 15;
        public const byte CREATE_PROJECTILE_REPLICANT = 16;

        public const byte UPDATE_HIT_POINTS = 17;

        public const byte DROP_LOOT_ITEM = 18;
    }
}