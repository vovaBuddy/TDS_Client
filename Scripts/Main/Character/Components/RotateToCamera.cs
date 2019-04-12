using UnityEngine;

namespace Main.Characters.Components
{
    public class RotateToCamera : MonoBehaviour
    {
        private void Update()
        {
            gameObject.transform.LookAt(Camera.main.transform);
        }
    }
}