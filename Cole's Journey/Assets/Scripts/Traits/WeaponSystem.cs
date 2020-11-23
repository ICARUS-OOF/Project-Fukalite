using ProjectFukalite.Handlers;
using ProjectFukalite.ScriptableObjects;
using ProjectFukalite.Enums;
using ProjectFukalite.Data;
using ProjectFukalite.Movement;
using ProjectFukalite.Interfaces;
using System.Collections;
using UnityEngine;
namespace ProjectFukalite.Traits
{
    public class WeaponSystem : MonoBehaviour
    {
        [SerializeField] private Transform cam;
        [SerializeField] private WeaponHolder weaponHolder;
        [SerializeField] private CameraShakeProperties cameraShakeProperties;

        public Weapon currentWeapon;

        private float attackDelay = 0f;
        private Referencer currentWeaponRef;

        private void Start()
        {
            foreach (Transform child in weaponHolder.swordHolder)
            {
                child.gameObject.SetActive(true);
                if (child.tag == ConstantHandler.WEAPON_TAG)
                {
                    Referencer referencer = child.GetComponent<Referencer>();
                    referencer.gameObject.SetActive(false);
                }
            }
        }

        private void Update()
        {
            foreach (Transform child in weaponHolder.swordHolder)
            {
                if (child.tag == ConstantHandler.WEAPON_TAG)
                {
                    if (child.name.ToUpper() == currentWeapon.name.ToUpper())
                    {
                        child.gameObject.SetActive(true);
                        currentWeaponRef = child.GetComponent<Referencer>();
                    }
                    else
                    {
                        child.gameObject.SetActive(false);
                    }
                }
            }
            if (Input.GetKeyDown(KeyHandler.AttackKey) && Time.time >= attackDelay)
            {
                if (currentWeapon.weaponType == WeaponType.Sword)
                {
                    attackDelay = Time.time + 1f;
                    StartCoroutine(StartSwordAttack());
                }
            }
        }

        private IEnumerator StartSwordAttack()
        {
            currentWeaponRef.refObj.SetActive(true);
            weaponHolder.anim.SetTrigger("Attack" + Random.Range(1, 3).ToString());
            yield return new WaitForSeconds(.2f);
            PlayerReferencer.singleton.camMovement.ShakeCamera(cameraShakeProperties);
            yield return new WaitForSeconds(.2f);
            ProcessDamage();
            yield return new WaitForSeconds(.6f);
            currentWeaponRef.refObj.SetActive(false);
        }

        private void ProcessDamage()
        {
            RaycastHit _hitInfo;
            if (Physics.SphereCast(cam.transform.position, currentWeapon.damageSpread, cam.transform.forward, out _hitInfo, currentWeapon.range))
            {
                IEnemy enemy = _hitInfo.transform.GetComponent<IEnemy>();
                if (enemy != null)
                {
                    enemy.Damage(currentWeapon.damage);
                }
            }
        }
    }
}