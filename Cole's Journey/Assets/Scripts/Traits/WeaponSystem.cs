using ProjectFukalite.Handlers;
using ProjectFukalite.ScriptableObjects;
using ProjectFukalite.Enums;
using ProjectFukalite.Data;
using System.Collections;
using UnityEngine;
namespace ProjectFukalite.Traits
{
    public class WeaponSystem : MonoBehaviour
    {
        [SerializeField] private WeaponHolder weaponHolder;
        public Weapon currentWeapon;

        private float attackDelay = 0f;

        private void Update()
        {
            foreach (Transform child in weaponHolder.swordHolder)
            {
                if (child.tag == ConstantHandler.WEAPON_TAG)
                {
                    if (child.name == currentWeapon.name)
                    {
                        child.gameObject.SetActive(true);
                    }
                    else
                    {
                        child.gameObject.SetActive(true);
                    }
                }
            }
            if (Input.GetKeyDown(KeyHandler.AttackKey) && Time.time >= attackDelay)
            {
                if (currentWeapon.weaponType == WeaponType.Sword)
                {
                    attackDelay = Time.time + currentWeapon.attackRate;
                    StartCoroutine(StartAttack());
                }
            }
        }

        private IEnumerator StartAttack()
        {
            weaponHolder.anim.SetTrigger("Attack");
            yield return new WaitForSeconds(.1f);
            PlayerReferencer.singleton.camMovement.ShakeCamera(.6f, .4f);
        }
    }
}