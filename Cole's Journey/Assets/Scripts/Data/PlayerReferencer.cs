using ProjectFukalite.Movement;
using ProjectFukalite.Traits;
using UnityEngine;
namespace ProjectFukalite.Data
{
    public class PlayerReferencer : MonoBehaviour
    {
        #region Singleton
        public static PlayerReferencer singleton;
        private void Awake()
        {
            if (singleton == null)
            {
                singleton = this;
            }
        }
        #endregion

        public PlayerMovement playerMovement;
        public CameraMovement camMovement;

        public PlayerData playerData;

        public WeaponSystem weaponSystem;
        public WeaponHolder weaponHolder;
    }
}