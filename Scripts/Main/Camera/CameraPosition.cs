using System.Collections;
using System.Collections.Generic;
using Core.MessageBus;
using Main.Weapons.Configs;
using UnityEngine;

namespace Main.GameCamera
{
    public class CameraPosition : SubscriberBehaviour
    {
        GameObject owner;
        GameObject cursorPos;

        public CameraController cam;
        public float CameraSpeed;

        public float MIN_CAM_OFFSET;
        public float MAX_CAM_OFFSET;

        public float MIN_DIST;
        public float MAX_DIST;

        public CameraTargetConfig config;

        float dist;
        float dist_ampl;

        private void Start()
        {
            dist_ampl = MAX_DIST - MIN_DIST;
        }

        public void Init(GameObject player, GameObject cursor, CameraTargetConfig cfg)
        {
            owner = player;
            cursorPos = cursor;
            config = cfg;
        }

        private void Update()
        {
            dist = Vector3.Distance(owner.transform.position, cursorPos.transform.position);

            transform.position = Vector3.Lerp(transform.position,
                Vector3.Lerp(owner.transform.position, cursorPos.transform.position, 
                config.curveTargetPosByDist.Evaluate(dist)), 
                Time.deltaTime * CameraSpeed);

            cam.targetHeightOffset = config.curveOffsetByDist.Evaluate(dist) * (-100);
        }
    }
}