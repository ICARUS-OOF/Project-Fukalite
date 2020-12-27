using UnityEngine;
namespace ProjectFukalite.Utils
{
    public class CameraUtils : MonoBehaviour
    {
        private Transform camTransform;

        private float shakeIntesity = 0;

        [SerializeField] private Camera cam;

        private void Start()
        {
            camTransform = transform;
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
                Vector3 camPos = camTransform.position;

                float offsetX = Random.value * shakeIntesity * 2f - shakeIntesity;
                float offsetY = Random.value * shakeIntesity * 2f - shakeIntesity;

                camPos.x += offsetX;
                camPos.y += offsetY;

                camTransform.position = camPos;
            }
        }

        private void StopCameraShake()
        {
            CancelInvoke(nameof(StartCameraShake));
            camTransform.localPosition = Vector3.zero;
        }
    }
}