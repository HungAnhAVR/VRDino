using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public abstract class Enemy : Character {

	public bool isAttk;
	public bool isJumping;
	public bool isWalking;
	public bool isOnPath;
	public bool canFly;
	public bool hasAttacked; //has enemy attacked in a state?

	[HideInInspector] public Animator animator;
	[HideInInspector] public NavMeshAgent agent;
	[HideInInspector] public float initialSpeed; 

	public void Initialize()
	{
		animator = this.GetComponentInChildren<Animator> ();
		agent = this.GetComponent<NavMeshAgent> ();
		initialSpeed = agent.speed;
	}

	protected void Loop()
	{

			
	}

	public void Walk()
	{
		if (isWalking)
			return;
		
		isWalking = true;
		agent.isStopped = false;

		animator.SetBool ("walking",true);
		animator.SetBool ("jumping",false);
		animator.SetBool ("attacking",false);
	}

	public void Attack()
	{
		if (isAttk)
			return;
		
		isAttk = true;

		agent.isStopped = true;

		animator.SetBool ("walking",false);
		animator.SetBool ("jumping",false);
		animator.SetBool ("attacking",true);

	}

	public void Jump()
	{
		if (isJumping)
			return;

		isJumping = true;

		agent.speed = 1;

		animator.SetBool ("walking",false);
		animator.SetBool ("jumping",true);
		animator.SetBool ("attacking",false);
	}

	// Call by animation event
	public void StartJump()
	{
		agent.speed = 5; //propels forward
	}

	// Call by animation event
	public void EndJump()
	{
		agent.speed = initialSpeed; //resume walking
		isJumping = false;
		isWalking = false;
		animator.SetBool ("jumping",false);
	}

	// Call by animation event
	public void StartAttack()
	{
		transform.LookAt (Player.instance.transform);
	}

	// Call by animation event
	public void EndAttack()
	{
		isAttk = false;
		hasAttacked = true;
	}

	//Force enemy to follow a pre-defined path 
	public void FollowPath()
	{
		if (isOnPath)
			return;

		isOnPath = true;

		if(agent.isOnNavMesh)
		agent.isStopped = true;

		Random.seed = System.DateTime.Now.Millisecond;
		int rand = Random.Range (1,6);;
		print ("rand " + rand);
		string pathName = "path" + rand;

		iTween.MoveTo(gameObject, 
			iTween.Hash("path", iTweenPath.GetPath(pathName), 
				"orienttopath", true, 
				"looktime", 0.001f, 
				"lookahead", 0.001f, 
				"speed", 90, "easetype", 
				iTween.EaseType.linear, 
				"oncomplete", "OnCompletePath"));
	}

	void OnCompletePath()
	{
		isOnPath = false;
	}
		
}
