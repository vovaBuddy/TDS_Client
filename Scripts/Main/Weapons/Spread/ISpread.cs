using System.Collections;
using System.Collections.Generic;
using Main.Weapons.Configs;
using UnityEngine;

namespace Main.Weapons
{
    public interface ISpread
    {
        void Init(SpreadConfig config);
        Vector3 Get();
        void Increase(float deltaTime, bool first);
        void SetMoveK(float value);
        void SetAimK(float value);
    }
}