using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Main.Weapons.Configs
{
    [CreateAssetMenu(fileName = "CameraTargetConfig", menuName = "Config/CameraTargetConfig", order = 3)]
    public class CameraTargetConfig : ScriptableObject
    {
        #region CameraTarget
        [Header("Camera target info")]
        public AnimationCurve curveOffsetByDist;
        public AnimationCurve curveTargetPosByDist;
        #endregion
    }
}
