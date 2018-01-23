using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public abstract class Enemy : Character {

	public bool isAttk;
	public bool isJumping;
	public bool isWalking;

	[HideInInspector] public Animator animator;
	[HideInInspector] public NavMeshAgent agent;
	[HideInInspector] public float initialSpeed; 

	public void Initialize()
	{
		animator = this.GetComponentInChildren<Animator> ();
		agent = this.GetComponent<NavMeshAgent> ();
		initialSpeed = agent.speed;
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
		transform.LookAt (Player.instance.transform);

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
		
	}

	// Call by animation event
	public void EndAttack()
	{
		isAttk = false;
	}
		
}
