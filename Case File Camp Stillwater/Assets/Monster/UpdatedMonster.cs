using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class UpdatedMonster : MonoBehaviour
{
	// settings for monster
	public Transform player;
	public float walkSpeed, chaseSpeed, chaseRange, killDistance;
	public List<Transform> destinations;

	// Sound stuff
	public AudioSource footstepSource;
	public AudioSource screamSource;
	public List<AudioClip> footstepClips;
	public List<AudioClip> moans;
	private float footstepInterval = 1.2f;
	private float moanInterval = 10f;
	private float footstepTimer;
	private float moanTimer;
	public AudioClip attackScream;
	public AudioClip fleeScream;
	private bool hasPlayedAttackSound = false;
	private bool hasPlayedFleeSound = false;

	// Agent and navigation
	private Transform destination;
	private int randNum;
	private NavMeshAgent agent;
	private bool isHunting, isChasing, isFleeing;
	private Animator animator;

	public bool needsToKillPlayer = false;

	private const float RunAwaySpeed = 40;

	

	void Start()
	{
		agent = GetComponent<NavMeshAgent>();
		animator = GetComponent<Animator>();
		Hunting();
	}

	void Update()
	{
		footstepTimer -= Time.deltaTime;
		if (footstepTimer <= 0f)
		{
			PlayFootstep();
			footstepTimer = footstepInterval;
		}
		CheckPlayerProximity();
		if (needsToKillPlayer)
		{
			HuntAndKillNow();
		}
		else if (!needsToKillPlayer && isChasing)
		{
			// Reset chase state
			isChasing = false;
			isHunting = true;
			agent.ResetPath();
			Hunting();
		}
		else if (isFleeing)
		{
			agent.speed = RunAwaySpeed;
		}
		else if (isHunting)
		{
			moanTimer -= Time.deltaTime;
			if (moanTimer <= 0f)
			{
				PlayMoan();
				moanTimer = moanInterval;
			}
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

	void PlayFootstep()
	{
		if (footstepClips.Count > 0 && footstepSource != null)
		{
			AudioClip clip = footstepClips[Random.Range(0, footstepClips.Count)];
			footstepSource.PlayOneShot(clip);
		}
	}

	void PlayMoan()
	{
		if (footstepClips.Count > 0 && footstepSource != null)
		{
			AudioClip clip = moans[Random.Range(0, moans.Count)];
			footstepSource.PlayOneShot(clip);
		}
	}

	void HuntAndKillNow()
	{
		isChasing = true;
		isHunting = false;
		Debug.Log("Finding and killing player");
		agent.speed = chaseSpeed;
		agent.destination = player.position;
		animator.SetBool("isChasing", true);
		footstepInterval = .4f;
		if (!hasPlayedAttackSound)
		{
			screamSource.PlayOneShot(attackScream);
			hasPlayedAttackSound = true;
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
		animator.SetBool("isChasing", true);
		footstepInterval = .4f;
		if (!hasPlayedAttackSound)
		{
			screamSource.PlayOneShot(attackScream);
			hasPlayedAttackSound = true;
		}
	}

	void Hunting()
	{
		Debug.Log("HUNTING");
        agent.speed = walkSpeed; 
		hasPlayedAttackSound = false;
		hasPlayedFleeSound = false;
		footstepInterval = 1.2f;
		animator.SetBool("isChasing", false);
		if (!agent.hasPath || agent.velocity.sqrMagnitude == 0f)
		{
			PickNewDestination();
		}
	}

void KillPlayer()
{
    Debug.Log("Player is dead");

    // Show end screen
    GameManager.Instance.PlayerDied();

    // Optionally disable player movement
    GameObject playerObj = GameObject.FindGameObjectWithTag("Player");
    CharacterController controller = playerObj.GetComponent<CharacterController>();
    if (controller != null)
    {
        controller.enabled = false;
    }

    // You can also disable the camera/controller scripts here if needed
}


	void RunFromPlayer()
	{
		Debug.Log("Monster was hit with light");
		hasPlayedFleeSound = true;
		isFleeing = true;
		screamSource.PlayOneShot(fleeScream);
		Transform furthestPoint = GetFurthestPoint(agent.transform.position);
        if (furthestPoint != null)
        {
			agent.SetDestination(furthestPoint.position);
        }
	}

	void CheckPlayerProximity()
	{
		float distanceToPlayer = Vector3.Distance(transform.position, player.position);
		if (isFleeing)
		{
			Debug.Log("Monster is fleeing");
			if (!agent.pathPending && agent.remainingDistance <= agent.stoppingDistance)
			{
				isFleeing = false;
				isHunting = true;
				isChasing = false;
				Debug.Log("Monster is not fleeing and has reached it's destination to hunt again'");
			}
		}
		else if (distanceToPlayer < chaseRange)
		{
			isChasing = true;
			isHunting = false;
			Debug.Log("Chasing Player");
			if (distanceToPlayer < killDistance)
			{
				KillPlayer();
			}
		}
		else
		{
			isChasing = false;
			isHunting = true;
			animator.SetBool("isChasing", false);
		}
	}

	void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Light"))
        {
			RunFromPlayer();
        }
    }

	// Helper method for monster escaping
	Transform GetFurthestPoint(Vector3 fromPosition)
    {
        Transform furthest = null;
        float maxDistance = 0f;

        foreach (Transform point in destinations)
        {
            float dist = Vector3.Distance(fromPosition, point.position);
            if (dist > maxDistance)
            {
                maxDistance = dist;
                furthest = point;
            }
        }

        return furthest;
    }

}