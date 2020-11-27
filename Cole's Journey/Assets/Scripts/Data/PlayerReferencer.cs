using ProjectFukalite.Movement;
using ProjectFukalite.Traits;
using UnityEngine;
using UnityEngine.UI;
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

        [Header("Script References")]
        public PlayerMovement playerMovement;
        public CameraMovement camMovement;

        public PlayerData playerData;

        public WeaponSystem weaponSystem;
        public WeaponHolder weaponHolder;

        [Header("Component References")]
        public Slider healthSlider;
        public Slider staminaSlider;
    }
}