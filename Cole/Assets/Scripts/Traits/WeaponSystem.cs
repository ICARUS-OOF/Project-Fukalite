using ProjectFukalite.Handlers;
using ProjectFukalite.ScriptableObjects;
using ProjectFukalite.Enums;
using ProjectFukalite.Data;
using ProjectFukalite.Movement;
using ProjectFukalite.Interfaces;
using ProjectFukalite.Systems;
using ProjectFukalite.Data.Containment;
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

        private bool isAttacking = false;
        private bool isBlocking = false;

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
            if (PlayerUI.singleton.isPanel)
            { return; }

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
            if (Input.GetKey(KeyHandler.BlockKey) && !isAttacking)
            {
                Block();
            } else
            {
                Unblock();
            }
            if (Input.GetKeyDown(KeyHandler.AttackKey) && !isAttacking && !isBlocking)
            {
                if (currentWeapon.weaponType == WeaponType.Sword)
                {
                    StartCoroutine(StartSwordAttack());
                }
            }
        }

        private IEnumerator StartSwordAttack()
        {
            isAttacking = true;
            currentWeaponRef.refObj.SetActive(true);
            weaponHolder.anim.SetTrigger("Attack" + Random.Range(1, 3).ToString());
            yield return new WaitForSeconds(.2f);
            PlayerReferencer.singleton.camMovement.ShakeCamera(cameraShakeProperties);
            yield return new WaitForSeconds(.2f);
            ProcessDamage();
            yield return new WaitForSeconds(.2f);
            currentWeaponRef.refObj.SetActive(false);
            isAttacking = false;
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

        private void Block()
        {
            isBlocking = true;
            weaponHolder.anim.SetBool("Block", true);
        }

        private void Unblock()
        {
            isBlocking = false;
            weaponHolder.anim.SetBool("Block", false);
        }
    }
}