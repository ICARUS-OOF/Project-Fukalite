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
using ProjectFukalite.Triggers;

namespace ProjectFukalite.Traits
{
    public class WeaponSystem : MonoBehaviour
    {
        [SerializeField] private Transform cam;
        [SerializeField] private WeaponHolder weaponHolder;
        [SerializeField] private CameraShakeProperties cameraShakeProperties;

        public Weapon currentWeapon;

        private bool isAttacking = false;
        public bool isBlocking = false;

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
            Sword _sword = currentWeaponRef.GetComponent<Sword>();
            isAttacking = true;
            currentWeaponRef.refObj.SetActive(true);
            weaponHolder.anim.SetTrigger("Attack" + Random.Range(1, 3).ToString());
            yield return new WaitForSeconds(.2f);
            PlayerReferencer.singleton.camMovement.ShakeCamera(cameraShakeProperties);
            yield return new WaitForSeconds(.4f);
            _sword.ClearColliders();
            currentWeaponRef.refObj.SetActive(false);
            isAttacking = false;
        }

        public void SetWeapon(Weapon _weapon)
        {
            currentWeapon = _weapon;
        }

        public void ProcessDamage(Collider col)
        {
            AudioSource impactSource = currentWeaponRef.refObj2.GetComponent<AudioSource>();

            IEnemy enemy = col.transform.GetComponent<IEnemy>();
            if (enemy != null)
            {
                impactSource.clip = AudioHandler.GetSoundEffect("Sword Impact " + Random.Range(1, 3)).clip;
                impactSource.Play();
                enemy.Damage(currentWeapon.damage);
            } 

            EnemyReferencer enemyRef = col.transform.GetComponent<EnemyReferencer>();
            if (enemyRef != null)
            {
                impactSource.clip = AudioHandler.GetSoundEffect("Sword Impact " + Random.Range(1, 3)).clip;
                impactSource.Play();
                enemyRef.Damage(currentWeapon.damage);
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