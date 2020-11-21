using UnityEngine;
using ProjectFukalite.Enums;
namespace ProjectFukalite.ScriptableObjects
{
    [CreateAssetMenu(menuName = "Data/Weapon", fileName = "New Weapon")]
    public class Weapon : ScriptableObject
    {
        public int damage = 30;
        public float attackRate;
        public WeaponType weaponType;
    }
}