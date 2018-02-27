using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using RootMotion.Dynamics;
using RootMotion.FinalIK;

public abstract class Enemy : Character {

	public bool isOnPath;
	public bool isGrounded;
	public float attkRate;

	public ENEMY_STATE animState;
	public HitReaction hitReaction;
	public FullBodyBipedIK bodyIK;
	public Transform puppet;

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
		DIE
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

		if (animState == ENEMY_STATE.DIE)
			return;
		
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
		//Steer ();
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
		//Steer();

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

	public void Hit(Collider hitCollider,Vector3 collisionPoint,float impact)
	{
		if (!isHit) {
			
			isHit = true;

			int damage = 5 * (int)impact + Random.Range(-10,10);

			// Run hit animation
			Vector3 dir = hitCollider.transform.position - collisionPoint;
			hitReaction.Hit (hitCollider,dir.normalized * impact/6 ,collisionPoint);

			// Show hit number pop up
			ShowHitNumber (damage);

			// Calculate damage
			OnHit (damage);

			// Play Effect 
			HitEffect(collisionPoint);

			print ("damage " + damage + " impact "+impact);
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
		Vector3 lookPos = destination - transform.position;
		lookPos.y = 0;
		Quaternion rotation = Quaternion.LookRotation(lookPos);
		transform.rotation = Quaternion.Slerp(transform.rotation, rotation, 2);  
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
		Vector3 fwd = transform.position + new Vector3(0,1,0);
		fwd = transform.TransformDirection(Vector3.forward);

		if (Physics.Raycast (transform.position + transform.forward + new Vector3 (0, 1, 0), fwd, out hit, 10)) {

			if (hit.transform.tag == "Enemy") {

				if (rand == 0) {
					agent.Move (transform.right * Time.deltaTime * 1);
				} else {
					agent.Move (-transform.right* Time.deltaTime * 1);
				}

				print("There is something in front of the object!" + hit.transform.name);
			}

		}

		Debug.DrawLine (transform.position + transform.forward + new Vector3 (0, 1, 0) ,transform.transform.position + transform.forward * 35, Color.blue);
	}

	protected override void Die()
	{
		if (animState == ENEMY_STATE.DIE)
			return;
		
		animState = ENEMY_STATE.DIE;
		Player.instance.enemyNo--;
		stateController.enabled = false;
		obs.enabled = false;
		agent.enabled = false;

		DieEffect();
		print("DIE");
		ApplyPhysics ();
	}


	protected virtual void ApplyPhysics()
	{

	}

	public void Blast(Vector3 center)
	{
		if (animState == ENEMY_STATE.DIE)
			return;
		
		OnHit (1000);
		// Show hit number pop up
		ShowHitNumber (1000);

		animState = ENEMY_STATE.DIE;
	}
		
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
	float hitFreq = .15f;
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

	void ShowHitNumber(int damage)
	{
		HitNumber hitNumber = ObjectPool.instance.GetHitNumber ();
		if (hitNumber != null) {
			hitNumber.transform.position = hitReaction.transform.position;
			hitNumber.transform.position += Random.insideUnitSphere * .25f;
			hitNumber.Show (damage);
		}
	}

	public void HitEffect(Vector3 pos)
	{
		TextHitRandom hitRand = ObjectPool.instance.GetTextHitRandom ();
		if (hitRand != null) {
			hitRand.transform.position = pos;
			hitRand.Live ();
		}
	}

	protected void DieEffect()
	{
		DeathSkull ds = ObjectPool.instance.GetDeathSkulls ();
		if (ds != null) {
			ds.transform.position = puppet.transform.position;
			ds.Live ();
		}
	}
}
