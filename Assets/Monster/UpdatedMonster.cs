using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class UpdatedMonster : MonoBehaviour
{
	public Transform player;
	public float walkSpeed, chaseSpeed;
	private bool isHunting, isChasing;
	public List<Transform> destinations;

	private Transform destination;
	private int randNum;
	private NavMeshAgent agent;

	private float waitTime = 3f;
	private bool isWaiting = false;

	void Start()
	{
		agent = GetComponent<NavMeshAgent>();
		isHunting = true;
		agent.speed = walkSpeed;
	}

	void Update()
	{
		if (isHunting)
		{
			if (!agent.hasPath || agent.velocity.sqrMagnitude == 0f)
			{
				PickNewDestination();
			}
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

}