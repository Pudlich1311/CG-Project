
using UnityEngine;
using UnityEngine.AI;

using StarterAssets;
using System.Collections;

public class Drone : MonoBehaviour
{
    public NavMeshAgent agent;

    private Transform player;

    public LayerMask whatIsGround, whatIsPlayer;

    public float health;

    public int dmg;

    public Animator an;
    public CurrentState curr;

    public enum CurrentState
    {
        Walking,
        Attacking,
        Death
    }

    //Patroling
    public Vector3 walkPoint;
    bool walkPointSet;
    public float walkPointRange;

    //Attacking
    public float timeBetweenAttacks;
    bool alreadyAttacked;
    public GameObject projectile;
    public float bulletForce;

    //States
    public float sightRange, attackRange;
    public bool playerInSightRange, playerInAttackRange;

    private void Awake()
    {
        player = GameObject.Find("Capsule").transform;
        agent = GetComponent<NavMeshAgent>();
    }

    private void Update()
    {
        //Check for sight and attack range
        playerInSightRange = Physics.CheckSphere(transform.position, sightRange, whatIsPlayer);
        playerInAttackRange = Physics.CheckSphere(transform.position, attackRange, whatIsPlayer);

        if (!playerInSightRange && !playerInAttackRange) Patroling();
        if (playerInSightRange && !playerInAttackRange) ChasePlayer();
        if (playerInAttackRange && playerInSightRange) AttackPlayer();
    }

    private void Patroling()
    {
        if (!walkPointSet) SearchWalkPoint();

        if (walkPointSet)
            agent.SetDestination(walkPoint);

        Vector3 distanceToWalkPoint = transform.position - walkPoint;

        curr = CurrentState.Walking;
        AnimationCkeck();

        //Walkpoint reached
        if (distanceToWalkPoint.magnitude < 1f)
            walkPointSet = false;
    }
    private void SearchWalkPoint()
    {
        //Calculate random point in range
        float randomZ = Random.Range(-walkPointRange, walkPointRange);
        float randomX = Random.Range(-walkPointRange, walkPointRange);

        walkPoint = new Vector3(transform.position.x + randomX, transform.position.y, transform.position.z + randomZ);

        if (Physics.Raycast(walkPoint, -transform.up, 2f, whatIsGround))
            walkPointSet = true;
    }

    private void ChasePlayer()
    {
        agent.SetDestination(player.position);
        curr = CurrentState.Walking;
        AnimationCkeck();
    }

    private void AttackPlayer()
    {
        //Make sure enemy doesn't move
        agent.SetDestination(transform.position);

        transform.LookAt(player);

        float x = Random.Range(-0.07f, 0.07f);
        float y = Random.Range(-0.07f, 0.07f);

        //Calculate Direction with Spread
        Vector3 direction = transform.forward + new Vector3(x, y, 0);

        curr = CurrentState.Attacking;
        AnimationCkeck();

        RaycastHit hit;
        Ray ray = new Ray(transform.position, transform.forward);

        if (!alreadyAttacked)
        {
            if (Physics.Raycast(transform.position, direction, out hit, 20.0f))
            {
                GameObject laser = Instantiate(projectile, transform.position + transform.forward, transform.rotation) as GameObject;
                laser.GetComponent<ShotBehavior>().setTarget(hit.point);
                GameObject.Destroy(laser,2f);

                if (hit.collider.CompareTag("Player"))
                {
                    Debug.Log(hit.collider.name);
                    GameObject.Destroy(laser, 0.05f);

                    hit.collider.GetComponentInParent<FirstPersonController>().damage(dmg);
                   
                }
            }

           // Destroy(bullet.gameObject, 2);
            alreadyAttacked = true;
            Invoke(nameof(ResetAttack), timeBetweenAttacks);
        }
    }
    private void ResetAttack()
    {
        alreadyAttacked = false;
    }

    public void TakeDamage(int damage)
    {
        health -= damage;

        if (health <= 0) Invoke(nameof(DestroyEnemy), 0.5f);
    }
    private void DestroyEnemy()
    {
        GameObject.Find("LevelGenerator").GetComponent<Generation>().enemies -= 1;
        alreadyAttacked = true;
        curr = CurrentState.Death;
        AnimationCkeck();
        Destroy(gameObject);

    }


    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, attackRange);
        Gizmos.color = Color.yellow;
        Gizmos.DrawWireSphere(transform.position, sightRange);
    }

    private void AnimationCkeck()
    {

        if(curr== CurrentState.Walking)
        {
            an.SetBool("Attack",false);
            an.SetBool("Forward", true);
        }
        else if(curr == CurrentState.Attacking)
        {
            an.SetBool("Forward", false);
            an.SetBool("Attack", true);
        }
        else if(curr == CurrentState.Death)
        {
            an.SetBool("Forward", false);
            an.SetBool("Attack", false);
            an.SetBool("Death", true);
        }

    }
    


}
