using ProjectFukalite.Movement;
using ProjectFukalite.Traits;
using ProjectFukalite.Utils;
using UnityEngine;
using UnityEngine.UI;
namespace ProjectFukalite.Data.Containment
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

        public CameraUtils camUtils;

        public WeaponSystem weaponSystem;
        public WeaponHolder weaponHolder;

        [Header("Object References")]
        public GameObject cam;

        [Header("Component References")]
        public Rigidbody playerRigidBody;

        public Slider healthSlider;
        public Image staminaSlider;

        [Header("Text References")]
        public Text healthText;
        public Text staminaText;
    }
}