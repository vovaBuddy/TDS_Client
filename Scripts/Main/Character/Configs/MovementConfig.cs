using UnityEngine;

namespace Main.Characters.Configs
{
    [CreateAssetMenu(fileName = "MovementConfig", menuName = "Config/MovementConfig")]
    public class MovementConfig : ScriptableObject
    {
        public float RotateSpeed = 10.0f;
        public float RunSpeed = 5f;
        public float SprintSpeed = 7f;
        public float SideSpeed = 2f;
    }
}