using System.Collections;
using System.Collections.Generic;
using UnityEngine;
namespace ProjectFukalite.Utils
{
    public class CameraUtils : MonoBehaviour
    {
        private Transform cam;

        private float shakeIntesity = 0;

        private void Start()
        {
            cam = transform;
        }

        public void ShakeCamera(float intensity, float duration)
        {
            shakeIntesity = intensity;
            InvokeRepeating(nameof(StartCameraShake), 0, .01f);
            Invoke(nameof(StopCameraShake), duration);
        }

        private void StartCameraShake()
        {
            if (shakeIntesity > 0)
            {
                Vector3 camPos = cam.position;

                float offsetX = Random.value * shakeIntesity * 2f - shakeIntesity;
                float offsetY = Random.value * shakeIntesity * 2f - shakeIntesity;

                camPos.x += offsetX;
                camPos.y += offsetY;

                cam.position = camPos;
            }
        }

        private void StopCameraShake()
        {
            CancelInvoke(nameof(StartCameraShake));
            cam.localPosition = Vector3.zero;
        }
    }
}