using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class Monster : MonoBehaviour
{
    public Transform target;  // Target to move towards (like the player)
    private NavMeshAgent agent;  // The NavMeshAgent component

    void Start()
    {
        agent = GetComponent<NavMeshAgent>();  // Get the NavMeshAgent component
    }

    void Update()
    {
        if (target != null)
        {
            agent.SetDestination(target.position);  // Set the target destination
        }
    }
}
