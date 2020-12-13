using ProjectFukalite.Data.Containment;
using ProjectFukalite.Interfaces;
using ProjectFukalite.Handlers;
using ProjectFukalite.Systems;
using UnityEngine;
using UnityEngine.AI;
using System.Collections;
namespace ProjectFukalite.Entities
{
    public class Golem : MonoBehaviour, IEnemy
    {
        private NavMeshAgent agent;
        private PlayerReferencer referencer;
        private Animator anim;
        private AudioSource audioSource;

        public GameObject rockParticles;

        private Transform playerTransform;
        public Transform particlePoint;
        public Transform[] colTransforms;
        public AudioSource[] smashSFXSources;
        public ParticleSystem[] smashParticles;
        public GameObject[] disableOnDeath;

        public LayerMask groundLayer, playerLayer;

        public Vector3 walkPoint;

        public int damage = 20;

        private bool walkPointSet;
        public float walkPointRange;
        public float damageRange;

        public float attackDelay;

        public float timeBtwAttacks;
        private bool alreadyAttacked;

        public float sightRange, attackRange;
        public bool playerSighted, playerInRange;

        public float MaxHealth = 100;
        public float Health { get; set; }
        public bool isDead { get; set; }

        private void Awake()
        {
            agent = GetComponent<NavMeshAgent>();
        }

        private void Start()
        {
            referencer = PlayerReferencer.singleton;
            playerTransform = referencer.playerMovement.transform;
            anim = GetComponent<Animator>();
            Health = MaxHealth;
            audioSource = GetComponent<AudioSource>();
        }

        private void Update()
        {
            if (PlayerUI.singleton.isPanel || isDead)
            {
                if (PlayerUI.singleton.isPanel)
                {
                    audioSource.Pause();
                }
                return;
            }

            audioSource.UnPause();

            playerSighted = Physics.CheckSphere(transform.position, sightRange, playerLayer);
            playerInRange = Physics.CheckSphere(transform.position, attackRange, playerLayer);

            if (!playerSighted && !playerInRange) Patroling();
            if (playerSighted && !playerInRange) ChasePlayer();
            if (playerInRange && playerSighted) AttackPlayer();
        }

        private void Patroling()
        {
            anim.SetBool("Attack", false);

            anim.SetBool("Chasing", true);

            if (!walkPointSet) SearchWalkPoint();

            if (walkPointSet)
                agent.SetDestination(walkPoint);

            Vector3 distanceToWalkPoint = transform.position - walkPoint;

            if (distanceToWalkPoint.magnitude < 1f)
                walkPointSet = false;
        }

        private void SearchWalkPoint()
        {
            float randomZ = Random.Range(-walkPointRange, walkPointRange);
            float randomX = Random.Range(-walkPointRange, walkPointRange);

            walkPoint = new Vector3(transform.position.x + randomX, transform.position.y, transform.position.z + randomZ);

            if (Physics.Raycast(walkPoint, -transform.up, 2f, groundLayer))
                walkPointSet = true;
        }

        private void ChasePlayer()
        {
            anim.SetBool("Attack", false);
            anim.SetBool("Chasing", true);  

            Vector3 lTargetDir = playerTransform.position - transform.position;
            lTargetDir.y = 0.0f;
            transform.rotation = Quaternion.RotateTowards(transform.rotation, Quaternion.LookRotation(lTargetDir), Time.fixedDeltaTime * 3f);

            if (agent.enabled)
            {
                agent.SetDestination(playerTransform.position);
            }
        }

        private void AttackPlayer()
        {
            anim.SetBool("Attack", true);

            if (agent.enabled)
            {
                agent.SetDestination(transform.position);
            }

            if (!alreadyAttacked)
            {
                StartCoroutine(_Damage());

                alreadyAttacked = true;

                Invoke(nameof(ResetAttack), timeBtwAttacks);
            }
        }

        private IEnumerator _Damage()
        {
            yield return new WaitForSeconds(attackDelay  * .9f);

            for (int i = 0; i < smashSFXSources.Length; i++)
            {
                smashSFXSources[i].Play();
            }
            
            for (int i = 0; i < smashParticles.Length; i++)
            {
                smashParticles[i].Play();
            }

            yield return new WaitForSeconds(attackDelay * .1f);

            referencer.camUtils.ShakeCamera(.1f, .2f);

            for (int a = 0; a < colTransforms.Length; a++)
            {
                Collider[] colliders = Physics.OverlapSphere(colTransforms[a].position, damageRange);
                for (int b = 0; b < colliders.Length; b++)
                { 
                    Collider obj = colliders[b];
                    if (obj.transform.tag == ConstantHandler.PLAYER_TAG)
                    {
                        referencer.playerRigidBody.AddForce(transform.forward * 40f, ForceMode.Impulse);
                        referencer.playerData.Damage(damage);
                    }
                }
            }
        }

        private void ResetAttack()
        {
            alreadyAttacked = false;
            anim.SetBool("Attack", false);
            ChasePlayer();
        }

        public void Damage(float _damage)
        {
            Health -= _damage;
            if (Health <= 0)
            {
                Die();
            }
        }

        public void Die()
        {
            isDead = true;
            audioSource.enabled = false;
            agent.enabled = false;
            AudioHandler.PlaySoundEffect("Debris");
            GameObject _particles = Instantiate(rockParticles, particlePoint.position, Quaternion.identity);
            Destroy(_particles, 5.5f);
            for (int i = 0; i < disableOnDeath.Length; i++)
            {
                disableOnDeath[i].SetActive(false);
            }
            Invoke(nameof(SelfDestruct), 5.5f);
        }

        private void SelfDestruct()
        {
            Destroy(this.gameObject);
        }

        private void OnDrawGizmosSelected()
        {
            Gizmos.color = Color.green;
            Gizmos.DrawWireSphere(transform.position, sightRange);

            Gizmos.color = Color.yellow;
            Gizmos.DrawWireSphere(transform.position, attackRange);

            for (int i = 0; i < colTransforms.Length; i++)
            {
                Gizmos.color = Color.red;
                Gizmos.DrawWireSphere(colTransforms[i].position, damageRange);
            }
        }
    }
}