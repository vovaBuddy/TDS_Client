using UnityEngine;
using System.Collections;

namespace Main.Looting
{
    public class RotateComponent : MonoBehaviour
    {
        void Update()
        {
            transform.Rotate(transform.up, Time.deltaTime * 10.0f);
        }
    }
}