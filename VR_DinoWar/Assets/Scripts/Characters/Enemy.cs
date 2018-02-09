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
	[HideInInspector] public NavMeshObstacle obs;

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
	}

	public void Initialize()
	{
		animator = this.GetComponent<Animator> ();
		agent = this.GetComponentInChildren<NavMeshAgent> ();
		stateController = this.GetComponentInChildren<StateController> ();
		obs = this.GetComponentInChildren<NavMeshObstacle> ();
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

			FaceTarget (Player.instance.transform.position);

			break;

		case ENEMY_STATE.ATTKING:
			break;

		case ENEMY_STATE.START_PATH:
			animState = ENEMY_STATE.PATHING;
			Pathing ();
			break;

		case ENEMY_STATE.PATHING:
			break;

		}

		TrackObstacle ();
		TrackHitFreq ();
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
		if (animState != ENEMY_STATE.WALKING) {
			animState = ENEMY_STATE.START_WALK;
		}
		//Smooth out turns
		Steer();

		if (agent.isOnOffMeshLink) {
			Jump ();
		}
	}

	public void Attack()
	{
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
		if (!isHit) {
			this.collisionPoint = collisionPoint;
			isHit = true;
			OnHit (25);
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

	//Force enemy to follow a pre-defined path 
	public void Pathing()
	{
		if(agent.isOnNavMesh)
		agent.isStopped = true;

		int rand = Random.Range (1,6);;
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

		if (!agent.enabled)
			return;

		// Cast a sphere wrapping character controller 10 meters forward
		// to see if it is about to hit anything.
		if (Physics.SphereCast (eyePosition.position, 2f, eyePosition.forward , out hit, 70))
		{
			distanceToObstacle = hit.distance;

			if (hit.transform.tag == "Enemy") {
//				print (transform.name + " spotted " +hit.transform.name);
				if (rand == 0) {
					agent.Move (body.transform.right * Time.deltaTime * (float)Random.Range(5,10)/10);
				} else {
					agent.Move (-body.transform.right * Time.deltaTime * (float)Random.Range(5,10)/10);
				}
			} 
		}

		Debug.DrawLine (eyePosition.position,eyePosition.transform.position + eyePosition.forward * 35, Color.blue);
	}

	protected override void Die()
	{
		Player.instance.enemyNo--;
		stateController.enabled = false;
		animator.enabled = false;
		puppetMaster.Kill (PuppetMaster.StateSettings.Default);
		agent.enabled = false;
		obs.enabled = false;
		ApplyPhysics ();
	}


	protected virtual void ApplyPhysics()
	{
		Vector3 dir = ( body.transform.position - collisionPoint);
		pelvisRigidbody.AddForceAtPosition (dir* 25000, collisionPoint);
	}

	public void Blast(Vector3 center)
	{
		OnHit (1000);
		pelvisRigidbody.AddExplosionForce (3000,center,100);
	}

	public Rigidbody pelvisRigidbody;

	void TrackObstacle()
	{
		if (animState == ENEMY_STATE.ATTKING || animState == ENEMY_STATE.START_ATTK || animState == ENEMY_STATE.IDLE) {

			if(agent.enabled)
				agent.enabled = false;
			if(!obs.enabled)
				obs.enabled = true;
			
		} else {
			if(!agent.enabled)
				agent.enabled = true;
			if(obs.enabled)
				obs.enabled = false;
		}

	}

	float hitTimecount = 0;
	float hitFreq = .5f;
	bool isHit;
	void TrackHitFreq()
	{
		if (isHit) {
			hitTimecount += Time.deltaTime;

			if (hitTimecount > hitFreq) {
				hitTimecount = 0;
				isHit = false;
			}

		}
	}
}
