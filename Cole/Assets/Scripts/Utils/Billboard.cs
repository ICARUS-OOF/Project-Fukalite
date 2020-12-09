using UnityEngine;
using ProjectFukalite.Data.Containment;
namespace ProjectFukalite.Utils
{
    public class Billboard : MonoBehaviour
    {
        private Camera cam;
        private void Start()
        {
            cam = PlayerReferencer.singleton.cam.GetComponent<Camera>();
        }
        void LateUpdate()
        {
            transform.LookAt(transform.position + cam.transform.rotation * Vector3.forward,
                    cam.transform.rotation * Vector3.up);
        }
    }
}