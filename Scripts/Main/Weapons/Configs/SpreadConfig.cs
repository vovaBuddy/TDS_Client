using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Main.Weapons.Configs
{
    [CreateAssetMenu(fileName = "SpreadConfig", menuName = "Config/SpreadConfig", order = 3)]
    public class SpreadConfig : ScriptableObject
    {
        #region Spread
        [Header("Spread info")]
        public float bulletSpreadX;
        public float bulletSpreadY;
        public float startSpreadReduceFactor;

        [Space(5)]
        [Header("Auto reduce spread")]
        public float StayWithSpread;
        public float SlowReduceSpreadSpeed;
        public float FastReduceSpreadSpeed;

        [Space(5)]
        [Header("Cursor reduce spread")]
        public float CursoreReduceSpeed;
        public float MaxDelta;
        #endregion
    }
}
