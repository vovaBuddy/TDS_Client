using System.Collections;
using System.Collections.Generic;
using Main.Weapons.Configs;
using UnityEngine;

namespace Main.Weapons
{
    public class StandardSpread : ISpread
    {
        Vector3 spread;
        SpreadConfig config;

        float Aim_K = 1.0f;
        float Move_K = 1.0f;
        float stateK = 1.0f;
        
        public const float AIM_ON_K = 0.52f;
        public const float AIM_OFF_K = 0.8f;
        public const float MOVE_OFF_K = 1.2f;
        public const float MOVE_ON_K = 1.8f;

        float FinalAccuracyModY;
        float FinalAccuracyModX;

        Vector3 additionalSpread;
        Vector3 curSpread;

        public Vector3 Get()
        {
            return spread;
        }

        public void Increase(float deltaTime, bool first)
        {
            //if (!first)
            //{
                additionalSpread.x = config.bulletSpreadX * stateK;
                additionalSpread.y = config.bulletSpreadY * stateK;
            //}
            //else
            //{
            //    additionalSpread.x = config.bulletSpreadX * stateK / config.startSpreadReduceFactor;
            //    additionalSpread.y = config.bulletSpreadY * stateK / config.startSpreadReduceFactor;
            //}

            curSpread = additionalSpread;

            FinalAccuracyModX = Random.Range(-curSpread.x, curSpread.x);

            //if (!first)
                FinalAccuracyModY = Random.Range(curSpread.y, 1.15f * curSpread.y);
            //else
            //    FinalAccuracyModY = Random.Range(-curSpread.y, curSpread.y);

            spread = new Vector3(FinalAccuracyModX, FinalAccuracyModY, 0);
        }

        public void Init(SpreadConfig cfg)
        {
            config = cfg;
            additionalSpread = Vector3.zero;
        }

        public void SetMoveK(float value)
        {
            Move_K = value;
            stateK = Move_K * Aim_K;
        }

        public void SetAimK(float value)
        {
            Aim_K = value;
            stateK = Move_K * Aim_K;
        }
    }
}
