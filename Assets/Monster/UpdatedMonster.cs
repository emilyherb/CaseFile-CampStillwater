using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class UpdatedMonster : MonoBehaviour
{
	// settings for monster
	public Transform player;
	public float walkSpeed, chaseSpeed, chaseRange;
	public List<Transform> destinations;

	// Agent and navigation
	private Transform destination;
	private int randNum;
	private NavMeshAgent agent;
	private bool isHunting, isChasing;

	

	void Start()
	{
		agent = GetComponent<NavMeshAgent>();
		Hunting();
	}

	void Update()
	{
		CheckPlayerProximity();
		if (isHunting)
		{
			if (!agent.hasPath || agent.velocity.sqrMagnitude == 0f)
			{
				Hunting();
			}
		}
		else if (isChasing)
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
		agent.speed = chaseSpeed;
		agent.destination = player.position;
	}

	void Hunting()
	{
		Debug.Log("HUNTING");
        agent.speed = walkSpeed; 
		if (!agent.hasPath || agent.velocity.sqrMagnitude == 0f)
		{
			PickNewDestination();
		}
	}

	void KillPlayer()
	{
		Debug.Log("Player is dead");
	}

	void CheckPlayerProximity()
	{
		float distanceToPlayer = Vector3.Distance(transform.position, player.position);
		if (distanceToPlayer < chaseRange)
		{
			isChasing = true;
			isHunting = false;
			Debug.Log("Chasing Player");
			if (distanceToPlayer < 2)
			{
				KillPlayer();
			}
		}
		else
		{
			isChasing = false;
			isHunting = true;
		}
	}

}