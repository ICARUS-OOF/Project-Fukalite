using ProjectFukalite.Data.Containment;
using ProjectFukalite.Interfaces;
using UnityEngine;
using UnityEngine.AI;
using System.Collections;
namespace ProjectFukalite.Entities
{
    public class Golem : MonoBehaviour, IEnemy
    {
        private NavMeshAgent agent;
        private PlayerReferencer referencer;

        private Transform playerTransform;

        public LayerMask groundLayer, playerLayer;

        public Vector3 walkPoint;
        private bool walkPointSet;
        public float walkPointRange;

        public float timeBtwAttacks;
        private bool alreadyAttacked;

        public float sightRange, attackRange;
        public bool playerSighted, playerInRange;

        public float Health { get; set; }
        public bool isDead { get; set; }

        private void Awake()
        {
            playerTransform = PlayerReferencer.singleton.playerMovement.transform;
            agent = GetComponent<NavMeshAgent>();
        }

        private void Start()
        {
            referencer = PlayerReferencer.singleton;
        }

        private void Update()
        {
            playerSighted = Physics.CheckSphere(transform.position, sightRange, playerLayer);
            playerInRange = Physics.CheckSphere(transform.position, attackRange, playerLayer);

            if (!playerSighted && !playerInRange) Patroling();
            if (playerSighted && !playerInRange) ChasePlayer();
            if (playerInRange && playerSighted) AttackPlayer();
        }

        private void Patroling()
        {
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
            agent.SetDestination(playerTransform.position);
        }

        private void AttackPlayer()
        {
            agent.SetDestination(transform.position);

            transform.LookAt(playerTransform);

            if (!alreadyAttacked)
            {
                RaycastHit _hitInfo;
                if (Physics.Raycast(referencer.cam.transform.position, referencer.cam.transform.forward, out _hitInfo, 5f))              
                {

                }

                alreadyAttacked = true;
                Invoke(nameof(ResetAttack), timeBtwAttacks);
            }
        }

        private void ResetAttack()
        {
            alreadyAttacked = false;
        }

        private IEnumerator Attack()
        {
            yield return null;
        }

        public void Damage(float _damage)
        {

        }

        public void Die()
        {

        }
    }
}