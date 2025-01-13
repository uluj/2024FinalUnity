using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class EnemyAIZombie : MonoBehaviour
{
    
    [SerializeField] private Transform target;
    [SerializeField] float chaseRange = 5f;

    public Animator animator;
    NavMeshAgent navMeshAgent;
    float distanceToTarget = Mathf.Infinity;
    bool isProvoked = false;

    // Start is called before the first frame update
    void Start()
    {
        navMeshAgent = GetComponent<NavMeshAgent>();
        
    }
    // Update is called once per frame
    void Update()
    {
        distanceToTarget = Vector3.Distance(target.position, transform.position);
        if (isProvoked)
        {
            if (distanceToTarget > chaseRange)
            {
                isProvoked = false;
                navMeshAgent.SetDestination(transform.position); // stop moving
                animator.SetBool("ZombieTriggered", false);
            }
            else
            {
                EngageTarget();
                Debug.Log("Animator Triggered should trigger now");
                animator.SetBool("ZombieTriggered", true);
            }
        }
        else if (distanceToTarget <= chaseRange)
        {
            isProvoked = true;
        }
        
    }

    private void EngageTarget()
    {
        if (distanceToTarget >= navMeshAgent.stoppingDistance)
        {
            ChaseTarget();
        }
        else
        {
            AttackTarget();
        }
    }
    
    private void AttackTarget()
    {
        Debug.Log(name + " is attacking " + target.name); 
    }

    private void ChaseTarget()
    { 
        navMeshAgent.SetDestination(target.position);
    }
    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, chaseRange);
    }
    
}
