using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Main.GameCamera
{
    public class CameraController : MonoBehaviour
    {
        [Header("Camera Basic Settings")]
        public float FollowSpeed;
        public Transform TargetToFollow;
        //Offset
        [HideInInspector]
        public float TargetHeight = 0.0f;

        public float targetHeightOffset;
        [HideInInspector]
        public float ActualHeight = 0.0f;

        public float HeightSpeed = 1.0f;
        public Transform CameraOffset;

        [Header("Shooting Shake Settings")]
        public bool isShaking = false;
        public float shakeFactor = 3f;
        public float shakeTimer = .2f;
        public float shakeSmoothness = 5f;
        [HideInInspector]
        public float actualShakeTimer = 0.2f;

        [Header("Explosion Shake Settings")]
        public bool isExpShaking = false;
        public float shakeExpFactor = 5f;
        public float shakeExpTimer = 1.0f;
        public float shakeExpSmoothness = 3f;
        [HideInInspector]
        public float actualExpShakeTimer = 1.0f;

        [Header("Movement Shake Settings")]
        public float movShaking = 1.0f;
        private Vector3 randomShakePos = Vector3.zero;

        private bool showBlood = true;

        // Use this for initialization
        void Start()
        {

            actualShakeTimer = shakeTimer;
            targetAngle = transform.rotation;

        }

        public Vector3 CalculateRandomShake(float shakeFac, bool isExplosion)
        {
            randomShakePos = new Vector3(Random.Range(-shakeFac, shakeFac), Random.Range(-shakeFac, shakeFac), Random.Range(-shakeFac, shakeFac));
            if (isExplosion)
                return randomShakePos * (actualExpShakeTimer / shakeExpTimer);
            else
                return randomShakePos * (actualShakeTimer / shakeTimer);
        }

        public void Shake(float factor, float duration)
        {
            isShaking = true;
            shakeFactor = factor;
            shakeTimer = duration;
            actualShakeTimer = shakeTimer;
        }

        public void ExplosionShake(float factor, float duration)
        {
            isExpShaking = true;
            shakeExpFactor = factor;
            shakeExpTimer = duration;
            actualExpShakeTimer = shakeExpTimer;
        }

        Quaternion targetAngle;
        float cameraRotateEnergy = 0;
        float cameraRotateEnergyDissipation = 0.001f;

        void LateUpdate()
        {
            if (Input.GetKeyDown(KeyCode.LeftAlt))
            {
                targetAngle = Quaternion.Euler(new Vector3(0, TargetToFollow.transform.parent.rotation.y, 0)) * transform.rotation;
            }

            if (Input.GetKeyDown(KeyCode.Q))
            {
                targetAngle = Quaternion.Euler(new Vector3(0, +45, 0)) * transform.rotation;
            }

            if (Input.GetKeyDown(KeyCode.E))
            {
                targetAngle = Quaternion.Euler(new Vector3(0, -45, 0)) * transform.rotation;
            }

            //if (cameraRotateEnergy != 0)
            //{
            //    targetAngle = transform.eulerAngles.y + cameraRotateEnergy*0.1f;

            //    if (Mathf.Abs(cameraRotateEnergy) < Mathf.Pow(cameraRotateEnergyDissipation, 1) * Time.deltaTime)
            //    {
            //        cameraRotateEnergy = 0;
            //    }
            //    else
            //    {
            //        if (cameraRotateEnergy > 0)
            //        {
            //            cameraRotateEnergy -= Mathf.Pow(cameraRotateEnergyDissipation, 1) * Time.deltaTime;
            //        }
            //        else
            //        {
            //            cameraRotateEnergy += Mathf.Pow(cameraRotateEnergyDissipation, 1) * Time.deltaTime;
            //        }
            //    }
            //}

            if (Quaternion.Angle(targetAngle, transform.rotation) > 0.005)
            {
                transform.rotation = Quaternion.Slerp(transform.rotation, targetAngle, 2 * FollowSpeed * Time.deltaTime);
                //transform.eulerAngles = Vector3.Lerp(transform.eulerAngles,

                //    new Vector3(transform.eulerAngles.x, targetAngle,
                //        transform.eulerAngles.z), 

                //    2 * FollowSpeed * Time.deltaTime);

            }

            if (TargetToFollow)
            {
                transform.position = Vector3.Lerp(transform.position, TargetToFollow.position, FollowSpeed * Time.deltaTime);

                ActualHeight = Mathf.Lerp(ActualHeight, TargetHeight + targetHeightOffset, Time.deltaTime * HeightSpeed);

                CameraOffset.localPosition = new Vector3(0.0f, 0.0f, ActualHeight);
            }

            if (isShaking && !isExpShaking)
            {
                if (actualShakeTimer >= 0.0f)
                {
                    actualShakeTimer -= Time.deltaTime;
                    Vector3 newPos = transform.localPosition + CalculateRandomShake(shakeFactor, false);
                    transform.localPosition = Vector3.Lerp(transform.localPosition, newPos, shakeSmoothness * Time.deltaTime);
                }
                else
                {
                    isShaking = false;
                    actualShakeTimer = shakeTimer;
                }
            }

            else if (isExpShaking)
            {
                if (actualExpShakeTimer >= 0.0f)
                {
                    actualExpShakeTimer -= Time.deltaTime;
                    Vector3 newPos = transform.localPosition + CalculateRandomShake(shakeExpFactor, true);
                    transform.localPosition = Vector3.Lerp(transform.localPosition, newPos, shakeExpSmoothness * Time.deltaTime);
                }
                else
                {
                    isExpShaking = false;
                    actualExpShakeTimer = shakeExpTimer;
                }
            }

        }
    }
}