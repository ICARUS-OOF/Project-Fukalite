using UnityEngine;
namespace ProjectFukalite.Data
{
    public class PlayerData : MonoBehaviour
    {
        public float Health = 100;
        [HideInInspector] public float MaxHealth = 100;
        public float Stamina = 100;
        [HideInInspector] public float MaxStamina = 100;

        private PlayerReferencer referencer;

        private void Start()
        {
            referencer = PlayerReferencer.singleton;

            Health = MaxHealth;
            Stamina = MaxStamina;
        }

        private void LateUpdate()
        {
            referencer.healthSlider.maxValue = MaxHealth;
            referencer.healthSlider.value = Health;

            referencer.staminaSlider.fillAmount = Stamina / 100;

            referencer.healthText.text = Health + "%";
            referencer.staminaText.text = Stamina + "%";
        }
    }
}