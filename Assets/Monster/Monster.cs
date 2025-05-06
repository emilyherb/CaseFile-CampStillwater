using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class Monster : MonoBehaviour
{
    public enum MonsterState
    {
        Hunting,  // Patrolling or looking for the player
        Chasing   // Chasing the player
    }

    public Transform target;  // Target (the player)
    public float chaseRange = 10f;  // Range to start chasing the player
    public float patrolSpeed = 3.5f;  // Patrol speed
    public float chaseSpeed = 5f;  // Chase speed
    public float patrolRadius = 20f;  // Radius in which the monster can patrol
    public float patrolInterval = 3f;  // Time interval for selecting a new random patrol destination

    private NavMeshAgent agent;  // The NavMeshAgent component
    private Vector3 patrolTarget;  // Current patrol destination
    private MonsterState currentState;  // Current state of the monster

    void Start()
    {
        agent = GetComponent<NavMeshAgent>();
        currentState = MonsterState.Hunting;  // Initially, the monster is hunting (patrolling)
        agent.speed = patrolSpeed;  // Set initial patrol speed
        StartCoroutine(Patrol());  // Start the random patrolling coroutine
    }

    void Update()
    {
        // Update the state logic
        switch (currentState)
        {
            case MonsterState.Hunting:
                HuntForPlayer();
                break;
            case MonsterState.Chasing:
                ChasePlayer();
                break;
        }
    }

    void HuntForPlayer()
    {
        // Check if the player is within chase range
        if (target != null && Vector3.Distance(transform.position, target.position) <= chaseRange)
        {
            // Switch to chasing state
            currentState = MonsterState.Chasing;
            agent.speed = chaseSpeed;  // Change the speed when chasing
            StopCoroutine(Patrol());  // Stop random patrolling when chasing
            return;  // Exit the hunting logic
        }
    }

    void ChasePlayer()
    {
        if (target != null)
        {
            agent.SetDestination(target.position);  // Set the destination to the player's position

            // If the player is out of chase range, switch back to hunting state
            if (Vector3.Distance(transform.position, target.position) > chaseRange)
            {
                currentState = MonsterState.Hunting;
                agent.speed = patrolSpeed;  // Reset speed to patrol
                StartCoroutine(Patrol());  // Restart random patrolling
            }
        }
    }

    // Coroutine for random patrolling within the patrol area
    IEnumerator Patrol()
    {
        while (currentState == MonsterState.Hunting)
        {
            // Set a random patrol target within a specified radius from the current position
            Vector3 randomPoint = transform.position + Random.insideUnitSphere * patrolRadius;
            NavMeshHit hit;
            
            // Make sure the random point is on the NavMesh
            if (NavMesh.SamplePosition(randomPoint, out hit, patrolRadius, NavMesh.AllAreas))
            {
                patrolTarget = hit.position;  // Set the patrol target to the valid position on the NavMesh
                agent.SetDestination(patrolTarget);  // Set the NavMeshAgent to move to this point
            }

            // Wait for the agent to reach the patrol target or for a new patrol point to be selected
            while (Vector3.Distance(transform.position, patrolTarget) > 1f)
            {
                yield return null;  // Continue until the target is reached
            }

            // Wait for a set interval before selecting a new patrol target
            yield return new WaitForSeconds(patrolInterval);
        }
    }
}
