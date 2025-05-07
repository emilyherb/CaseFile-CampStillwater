using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class UpdatedMonster : MonoBehaviour
{
	// settings for monster
	public Transform player;
	public float walkSpeed, chaseSpeed, chaseRange, chaseTime;
	public List<Transform> destinations;

	// Agent and navigation
	private Transform destination;
	private int randNum;
	private NavMeshAgent agent;
	private bool isHunting, isChasing;

	

	void Start()
	{
		agent = GetComponent<NavMeshAgent>();
		StopChasing();
	}

	void Update()
	{
		CheckPlayerProximity();
		if (isHunting)
		{
			if (!agent.hasPath || agent.velocity.sqrMagnitude == 0f)
			{
				PickNewDestination();
			}
		}
		if (isChasing)
		{
			StartChasing();
		}
	}

	void PickNewDestination()
	{
		if (destinations.Count == 0)
		{
			return;
		}

		randNum = Random.Range(0, destinations.Count);
		destination = destinations[randNum];
		agent.destination = destination.position;
	}

	void StartChasing()
	{
		Debug.Log("Chasing player");
		agent.speed = chaseSpeed;
		agent.destination = player.position;
	}

	void StopChasing()
	{
		Debug.Log("Stopping the chase...");
        isChasing = false;
        isHunting = true;
        agent.speed = walkSpeed; 
        PickNewDestination(); 
	}

	void CheckPlayerProximity()
	{
		float distanceToPlayer = Vector3.Distance(transform.position, player.position);
		if (distanceToPlayer < chaseRange)
		{
			isChasing = true;
			isHunting = false;
		}
		else
		{
			isChasing = false;
			isHunting = true;
		}
	}

}