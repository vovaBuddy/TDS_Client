using System;

namespace Main.Looting.API
{
    [Serializable]
    public enum LootType
    {
        WEAPONS = 0,
        BULLETS = 1,
        ABILITIES = 2,
        ATTACHMENTS = 3,
        CONSUMABLES = 4,
    }

    public enum ConsumableType
    {
        MEDKIT = 0,
        GRENADE = 1,
        SHILEDKIT = 2,
        SPECIAL = 3,
    }

    public static class Messages
    {
        
    }
}