using UnityEngine;
namespace ProjectFukalite.Movement
{
    public class CameraMovement : MonoBehaviour
    {
        public Transform playerBody;
        void Update()
        {
            transform.position = playerBody.transform.position;
        }
    }
}