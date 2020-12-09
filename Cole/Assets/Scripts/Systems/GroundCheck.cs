using UnityEngine;
using ProjectFukalite.Movement;
using ProjectFukalite.Data.Containment;
namespace ProjectFukalite.Systems
{
    public class GroundCheck : MonoBehaviour
    {
        [SerializeField] private Animator anim;
        [SerializeField] private ParticleSystem landingParticles;

        private PlayerMovement movement;

        private float FallDuration;

        private void Start()
        {
            movement = PlayerReferencer.singleton.playerMovement;
        }

        private void Update()
        {
            if (movement.grounded)
            {
                if (FallDuration >= 1.1f)
                {
                    TriggerLanding();
                }
                FallDuration = 0;
            }
            else
            {
                FallDuration += Time.deltaTime;
            }
        }

        private void TriggerLanding()
        {
            anim.SetTrigger("Landed");
            landingParticles.Play();
        }
    }
}