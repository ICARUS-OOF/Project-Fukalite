using UnityEngine;
namespace ProjectFukalite.Data
{
    public class PlayerData : MonoBehaviour
    {
        public int Health = 100;
        [HideInInspector] public int MaxHealth = 100;
        public float Stamina = 100;
        [HideInInspector] public float MaxStamina = 100;

        public float staminaRegainMultiplier = 2.5f;

        public float walkStaminaReductionMultiplier = 2f;
        public float dashStaminaReductionMultiplier = 3f;

        private PlayerReferencer referencer;

        private void Start()
        {
            referencer = PlayerReferencer.singleton;

            Health = MaxHealth;
            Stamina = MaxStamina;
        }

        private void Update()
        {
            Health = Mathf.Clamp(Health, 0, MaxHealth);
            Stamina = Mathf.Clamp(Stamina, 0f, MaxStamina);
        }

        private void LateUpdate()
        {
            referencer.healthSlider.maxValue = MaxHealth;
            referencer.healthSlider.value = Health;

            referencer.staminaSlider.fillAmount = Stamina / 100;

            referencer.healthText.text = Health + "%";
            referencer.staminaText.text = Mathf.Round(Stamina) + "%";
        }

        public void IncreaseStamina(float _amnt)
        {
            Stamina += _amnt;
        }

        public void ReduceStamina(float _amnt)
        {
            Stamina -= _amnt;
        }

        public void Heal(int _amnt)
        {
            Health += _amnt;
        }

        public void Damage(int _amnt)
        {
            Health -= _amnt;
        }
    }
}