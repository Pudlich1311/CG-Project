
using UnityEngine;
using UnityEngine.AI;

using StarterAssets;
using System.Collections;

public class Crab : MonoBehaviour
{
    public NavMeshAgent agent;

    private Transform player;

    public LayerMask whatIsGround, whatIsPlayer;

    public float health;

    public int dmg;

    private bool damaged;

    public Animator an;
    public CurrentState curr;

    bool dam;

    public enum CurrentState
    {
        Walking,
        Attacking,
        Death,
        AfterAttack
    }

    //Patroling
    public Vector3 walkPoint;
    bool walkPointSet;
    public float walkPointRange;

    //Attacking
    public float timeBetweenAttacks;
    bool alreadyAttacked;


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

        
        if (!alreadyAttacked)
        {
            curr = CurrentState.Attacking;
            AnimationCkeck();
            player.GetComponentInParent<FirstPersonController>().damage(dmg);

            alreadyAttacked = true;
            Invoke(nameof(ResetAttack), timeBetweenAttacks);
            StartCoroutine(AttackAnim());


        }


    }

    private IEnumerator AttackAnim()
    {
        yield return new WaitForSeconds(1);
        curr = CurrentState.AfterAttack;
        AnimationCkeck();
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


        if (curr == CurrentState.Walking)
        {
            an.SetBool("Attack", false);
            an.SetBool("Damaged", false);
            an.SetBool("Forward", true);
        }
        else if (curr == CurrentState.Attacking)
        {
            an.SetBool("Forward", false);
            an.SetBool("Damaged", false);
            an.SetBool("Attack", true);
        }
        else if (curr == CurrentState.Death)
        {
            an.SetBool("Forward", false);
            an.SetBool("Damaged", false);
            an.SetBool("Attack", false);
            an.SetBool("Death", true);
        }
        else if(curr == CurrentState.AfterAttack)
        {
            an.SetBool("Attack", false);
        }

    }



}
