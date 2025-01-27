using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class EnemyAIPuke : MonoBehaviour
{
    
    [SerializeField] private Transform target;
    [SerializeField] float chaseRange = 5f;

    public ParticleSystem puke;
    NavMeshAgent navMeshAgent;
    float distanceToTarget = Mathf.Infinity;
    bool isProvoked = false;

    // Start is called before the first frame update
    void Start()
    {
        navMeshAgent = GetComponent<NavMeshAgent>();
        puke.Pause();
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
                puke.Stop();
            }
            else
            {
                EngageTarget();
                puke.Play();
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
