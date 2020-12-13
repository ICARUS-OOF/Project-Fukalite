using ProjectFukalite.Data.Containment;
using ProjectFukalite.Traits;
using System.Collections.Generic;
using UnityEngine;
namespace ProjectFukalite.Triggers
{
    public class Sword : MonoBehaviour
    {
        private PlayerReferencer referencer;
        private WeaponSystem weaponSys;

        private List<Collider> cols = new List<Collider>();

        private void Start()
        {
            referencer = PlayerReferencer.singleton;
            weaponSys = referencer.weaponSystem;
        }

        private void OnTriggerEnter(Collider col)
        {
            if (cols.Contains(col))
            {
                return;
            }
            weaponSys.ProcessDamage(col);
            cols.Add(col);
        }

        public void ClearColliders()
        {
            cols.Clear();
        }
    }
}