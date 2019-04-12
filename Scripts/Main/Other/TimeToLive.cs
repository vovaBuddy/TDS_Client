using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Main.Other
{
    public class TimeToLive : MonoBehaviour
    {
        public float TTL;

        // Update is called once per frame
        void Update()
        {
            TTL -= Time.deltaTime;
            
            if(TTL <= 0) Destroy(gameObject);
        }
    }
}