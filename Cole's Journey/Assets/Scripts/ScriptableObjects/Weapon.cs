using UnityEngine;
using ProjectFukalite.Enums;
namespace ProjectFukalite.ScriptableObjects
{
    [CreateAssetMenu(menuName = "Data/Weapon", fileName = "New Weapon")]
    public class Weapon : ScriptableObject
    {
        public int damage = 30;
        public float range = 5f;
        public float damageSpread = 2f;
        public WeaponType weaponType;
    }
}