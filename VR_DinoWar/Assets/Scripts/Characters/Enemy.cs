using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using RootMotion.Dynamics;

public abstract class Enemy : Character {

	public bool isOnPath;
	public bool isGrounded;
	public float attkRate;

	public GameObject body;
	public Transform eyePosition;

	public ENEMY_STATE animState;

	public PuppetMaster puppetMaster;

	Vector3 collisionPoint;

	[HideInInspector] public Animator animator;
	[HideInInspector] public NavMeshAgent agent;
	[HideInInspector] public float initialSpeed; 

	protected StateController stateController;

	public float attkTime = 0;

	public enum ENEMY_STATE
	{
		IDLE = 0,
		START_WALK,
		WALKING ,
		START_JUMP,
		JUMPING,
		START_ATTK,
		ATTKING,
		START_PATH,
		PATHING,
		START_HIT,
		HITTING,
	}

	bool beingHit = false;

	public void Initialize()
	{
		animator = this.GetComponent<Animator> ();
		agent = this.GetComponentInChildren<NavMeshAgent> ();
		stateController = this.GetComponentInChildren<StateController> ();
		initialSpeed = agent.speed;
	}

	protected void Loop()
	{
		switch (animState) {

		case ENEMY_STATE.START_WALK:
			animState = ENEMY_STATE.WALKING;
			animator.SetInteger ("State", 1);
		
			break;

		case ENEMY_STATE.WALKING:
			
			break;

		case ENEMY_STATE.START_JUMP:
			animState = ENEMY_STATE.JUMPING;
			animator.SetInteger ("State", 2);
			break;

		case ENEMY_STATE.JUMPING:
			
			break;

		case ENEMY_STATE.START_ATTK:
			
			attkTime += Time.deltaTime;

			if (attkTime >= attkRate) {

				animState = ENEMY_STATE.ATTKING;
				animator.SetInteger ("State", 3);

			} else {
				Wait ();
			}

			FaceTarget (agent.destination);

			break;

		case ENEMY_STATE.ATTKING:

			break;

		case ENEMY_STATE.START_PATH:
			animState = ENEMY_STATE.PATHING;
			Pathing ();
			break;

		case ENEMY_STATE.PATHING:
			break;

		case ENEMY_STATE.START_HIT:
			animState = ENEMY_STATE.HITTING;
			animator.SetInteger ("State", -1);
			animator.SetTrigger ("Hit");

			OnHit (25);
			print ("OnHit");
			break;

		case ENEMY_STATE.HITTING:

		
			break;

		}
			
	}

	public void Wait()
	{
		animator.SetInteger ("State", 0);
	}

	public void Idle()
	{
		if (animState == ENEMY_STATE.IDLE) {
			return;
		}
		animState = ENEMY_STATE.IDLE;
		animator.SetInteger ("State", 0);
	}

	public void Walk()
	{
		if (beingHit)
			return;
		
		if (animState != ENEMY_STATE.WALKING) {
			animState = ENEMY_STATE.START_WALK;
		}
		//Smooth out turns
		agent.Move (transform.forward * Time.deltaTime * 2f);
	}

	public void Attack()
	{
		if (beingHit)
			return;
		if (animState != ENEMY_STATE.ATTKING) {
			animState = ENEMY_STATE.START_ATTK;
		}

	}

	public void Jump()
	{
		if (animState != ENEMY_STATE.JUMPING) {
			animState = ENEMY_STATE.START_JUMP;
		}
	}

	public void Hit(Vector3 collisionPoint)
	{
		if (animState != ENEMY_STATE.HITTING) {
			animState = ENEMY_STATE.START_HIT;
			beingHit = true;
			this.collisionPoint = collisionPoint;
		}
	}

	public void FollowPath()
	{
		if (animState != ENEMY_STATE.PATHING) {
			animState = ENEMY_STATE.START_PATH;
		}
	}

	// Call by animation event
	public void StartJump()
	{
		agent.speed = 1; //spring up
	}

	// Call by animation event
	public void MidJump()
	{
		agent.speed = 5; //propels forward
	}

	// Call by animation event
	public void EndJump()
	{
		agent.speed = initialSpeed; //resume speed
	}

	// Call by animation event
	public void StartAttack()
	{

	}

	// Call by animation event
	public void EndAttack()
	{
		attkTime = 0;
		Idle ();
	}

	// Call by animation event
	public void EndHit()
	{
		beingHit = false;
		Idle ();
	}

	//Force enemy to follow a pre-defined path 
	public void Pathing()
	{
		if(agent.isOnNavMesh)
		agent.isStopped = true;

		int rand = Random.Range (1,6);;
		print ("rand " + rand);
		string pathName = "path" + rand;

		iTween.MoveTo(gameObject, 
			iTween.Hash("path", iTweenPath.GetPath(pathName), 
				"orienttopath", true, 
				"looktime", 0.001f, 
				"lookahead", 0.001f, 
				"speed", 90, 
				"easetype", iTween.EaseType.linear, 
				"oncomplete", "OnCompletePath"));
	}

	void OnCompletePath()
	{
		
	}

	private void FaceTarget(Vector3 destination)
	{
		
		Vector3 lookPos = destination - body.transform.position;
		lookPos.y = 0;
		Quaternion rotation = Quaternion.LookRotation(lookPos);
		body.transform.rotation = Quaternion.Slerp(body.transform.rotation, rotation, 2);  
	}

	bool rolledRand;
	int rand ;

	//Attemp to steer away from other Enemy in front of this Enemy
	//or smooth out turns 
	public void Steer()
	{
		if (!rolledRand) {
			rand = Random.Range (0,2);
			rolledRand = true;
		}

		RaycastHit hit;

		float distanceToObstacle = 0;

		// Cast a sphere wrapping character controller 10 meters forward
		// to see if it is about to hit anything.
		if (Physics.SphereCast (eyePosition.position, .5f, transform.forward, out hit, 7))
		{
			
			distanceToObstacle = hit.distance;

			if (hit.transform.tag == "Enemy") {

				if (rand == 0) {
					agent.Move (transform.right * Time.deltaTime * 7);
				} else {
					agent.Move (-transform.right * Time.deltaTime * 7);
				}

			} 
				
		}
	
	}

	protected override void Die()
	{
		Player.instance.enemyNo--;
		stateController.enabled = false;
		animator.enabled = false;
		agent.isStopped = true;
		puppetMaster.Kill (PuppetMaster.StateSettings.Default);
		ApplyPhysics ();
	}


	protected virtual void ApplyPhysics()
	{
		Vector3 dir = ( body.transform.position - collisionPoint);
		testRB.AddForceAtPosition (dir* 30000, collisionPoint);
		agent.enabled = false;
	}

	public void Blast(Vector3 center)
	{
		OnHit (1000);
		testRB.AddExplosionForce (3000,center,100);
	}

	public Rigidbody testRB;
}
