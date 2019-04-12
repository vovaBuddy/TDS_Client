using Main.Characters.Configs;
using UnityEngine;

namespace Main.Characters.Data
{
    public enum SpeedMode
    {
        WOLK = 0, 
        RUN = 1, 
        SPRINT = 2
    }
    
    public interface ITransformData
    {
        SpeedMode CurrentSpeedMode { get; set; }
        MovementConfig MovementConfig { get; }
    }
}