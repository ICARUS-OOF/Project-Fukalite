using UnityEngine;
namespace ProjectFukalite.Traits
{
    public class WeaponHolder : MonoBehaviour
    {
        [SerializeField] private Transform weaponTargetTransform;
        [SerializeField] private Transform weaponCurrentTransform;

        public Transform swordHolder;

        [HideInInspector] public Animator anim;

        private void Start()
        {
            weaponCurrentTransform.position = weaponTargetTransform.position;
            weaponCurrentTransform.localRotation = weaponTargetTransform.rotation;
            anim = GetComponent<Animator>();
        }

        private void LateUpdate()
        {
            weaponCurrentTransform.position = Vector3.Lerp(weaponCurrentTransform.position, weaponTargetTransform.position, Time.fixedDeltaTime * 60f);
            weaponCurrentTransform.localRotation = Quaternion.Slerp(weaponCurrentTransform.localRotation, weaponTargetTransform.rotation, Time.fixedDeltaTime * 2f);
        }
    }
}