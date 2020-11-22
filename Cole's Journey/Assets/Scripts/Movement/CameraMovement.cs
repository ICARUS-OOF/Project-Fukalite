using UnityEngine;
namespace ProjectFukalite.Movement
{
    public class CameraMovement : MonoBehaviour
    {
        public Transform playerBody;

        [SerializeField] private Transform cam;

        private float shakeIntensity;

        void Update()
        {
            transform.position = playerBody.transform.position;
        }

        public void ShakeCamera(float intensity, float duration)
        {
            shakeIntensity = intensity;
            InvokeRepeating(nameof(StartCameraShake), 0, .01f);
            Invoke(nameof(StopCameraShake), duration);
        }

        private void StartCameraShake()
        {
            if (shakeIntensity > 0)
            {
                Vector3 camPos = cam.position;

                float offsetX = Random.value * shakeIntensity * 2f - shakeIntensity;
                float offsetY = Random.value * shakeIntensity * 2f - shakeIntensity;

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