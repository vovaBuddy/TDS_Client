using System;

namespace Main.Bullets.API
{
    public static class Messages
    {
        public const string CREATE_PROJECTILE = "Main.Bullets.CREATE_PROJECTILE";
        public const string CREATE_PROJECTILE_REPLICANT = "Main.Bullets.CREATE_PROJECTILE_REPLICANT";
        public const string PROJECTILE_COLLISION = "Main.Bullets.PROJECTILE_COLLISION";
    }
    
    [Serializable]
    public enum BulletType
    {
        LIGHT = 0,
        HEAVY = 1,
        SHELLS = 2,
    }
}