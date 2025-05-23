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
    public float patrolSpeed = 3.5f;  
    public float chaseSpeed = 5f;  
    public float patrolRadius = 20f;  // Radius in which the monster can patrol
    public float patrolInterval = 3f;  // Time interval for selecting a new random patrol destination

    private NavMeshAgent agent;  
    private Vector3 patrolTarget;  
    private MonsterState currentState;  // Current state of the monster

    public float viewRadius = 10f;
    [Range(0, 360)]
    public float viewAngle = 90f;
    public LayerMask targetMask;
    public LayerMask obstacleMask;

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
        if (CanSeeTarget())
        {
            currentState = MonsterState.Chasing;
            agent.speed = chaseSpeed;
            StopCoroutine(Patrol());
        }
    }

    bool CanSeeTarget()
    {
        if (target == null) return false;

        Vector3 directionToTarget = (target.position - transform.position).normalized;
        float distanceToTarget = Vector3.Distance(transform.position, target.position);

        // Check if within distance and angle
        if (distanceToTarget <= viewRadius)
        {
            float angleToTarget = Vector3.Angle(transform.forward, directionToTarget);
            if (angleToTarget <= viewAngle / 2f)
            {
                // Check if something is blocking view
                if (!Physics.Raycast(transform.position, directionToTarget, distanceToTarget, obstacleMask))
                {
                    return true;
                }
            }
        }

        return false;
    }

    Vector3 DirectionFromAngle(float angleDegrees, bool isGlobal)
    {
        if (!isGlobal)
            angleDegrees += transform.eulerAngles.y;

        return new Vector3(Mathf.Sin(angleDegrees * Mathf.Deg2Rad), 0, Mathf.Cos(angleDegrees * Mathf.Deg2Rad));
    }



    void ChasePlayer()
    {
        if (target != null)
        {
            agent.SetDestination(target.position);  // Set the destination to the player's position
            RotateTowards(target.position); // face the player

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
                RotateTowards(patrolTarget);
                yield return null;  // Continue until the target is reached
            }

            // Wait for a set interval before selecting a new patrol target
            yield return new WaitForSeconds(patrolInterval);
        }
    }

    void RotateTowards(Vector3 targetPosition)
    {
        Vector3 direction = (targetPosition - transform.position).normalized;
        direction.y = 0f;  // Prevent up/down tilting
        if (direction != Vector3.zero)
        {
            Quaternion lookRotation = Quaternion.LookRotation(direction);
            transform.rotation = Quaternion.Slerp(transform.rotation, lookRotation, Time.deltaTime * 5f);
        }
    }


    // for debugging only
    //TODO: Comment out when ready
    void OnDrawGizmos()
    {
        // Vision radius
        Gizmos.color = Color.yellow;
        Gizmos.DrawWireSphere(transform.position, viewRadius);

        // Draw vision cone lines
        Vector3 left = DirectionFromAngle(-viewAngle / 2, false);
        Vector3 right = DirectionFromAngle(viewAngle / 2, false);

        Gizmos.color = Color.blue;
        Gizmos.DrawLine(transform.position, transform.position + left * viewRadius);
        Gizmos.DrawLine(transform.position, transform.position + right * viewRadius);

        // Draw line to player if visible
        if (Application.isPlaying && CanSeeTarget())
        {
            Gizmos.color = Color.green;
            Gizmos.DrawLine(transform.position, target.position);
        }
    }




}
