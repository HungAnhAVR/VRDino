using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using RootMotion.Dynamics;
using RootMotion.FinalIK;

public abstract class Enemy : Character {

	public HitReaction hitReaction;
	public FullBodyBipedIK bodyIK;
	public Transform puppet;

	[HideInInspector] public Animator animator;
	[HideInInspector] public NavMeshAgent agent;
	[HideInInspector] public float initialSpeed; 
	[HideInInspector] public NavMeshObstacle obs;
	[HideInInspector] public bool isIdleDone;

	public delegate void OnIdleAnimOverEvent (StateController controller);
	public OnIdleAnimOverEvent onIdleAnimDone;

	public delegate void OnEndAttackAnim (StateController controller);
	public OnEndAttackAnim onEndAttackAnim;

	public bool isJumping;

	protected StateController stateController;

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
		TrackHitFreq ();

		if (stunTime > 0) {
			stunTime -= Time.deltaTime;
			if (stunTime <= 0) {
				stateController.AIEnabled = true;
				agent.isStopped = false;
			}
		}

		if (agent.isOnOffMeshLink) {
			if (!isJumping) {
				stateController.AIEnabled = false;
				agent.speed = 1;

				isJumping = true;

				animator.SetInteger ("State", 2);
				print ("JUMPING");
			}
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

	public float stunTime = 0;

	public void Stun()
	{
		stunTime += 1;
		stateController.AIEnabled = false;
		agent.isStopped = true;
		animator.SetInteger ("State", 4);
	}

	// Call by animation event
	public void StartJump()
	{

	}

	// Call by animation event
	public void MidJump()
	{
		agent.speed = 5;// propels forward
	}

	// Call by animation event
	public void EndJump()
	{
		StartCoroutine(JumpTouchUp());
	}

	IEnumerator JumpTouchUp()
	{
		agent.enabled = false;
		yield return new WaitForSeconds (.5f);
		agent.speed = initialSpeed; //resume speed
		agent.enabled = true;
		stateController.Resume();
	}

	// Call by animation event
	public void StartAttack()
	{

	}

	// Call by animation event
	public void EndAttack()
	{
		onEndAttackAnim (stateController);
	}

	// Call by animation event
	public void EndIdle()
	{
		isIdleDone = true;
		onIdleAnimDone (stateController);
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

	public void FaceTarget(Vector3 destination)
	{
		Vector3 lookPos = destination - transform.position;
		lookPos.y = 0;
		Quaternion rotation = Quaternion.LookRotation(lookPos);
		transform.rotation = Quaternion.Slerp(transform.rotation, rotation, 2);  
	}
		
	protected override void Die()
	{
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
		OnHit (1000);
		// Show hit number pop up
		ShowHitNumber (1000);
	}
		
	public void LocalAvoidanceOn()
	{
		agent.enabled = false;
		obs.enabled = true;
	}

	public void LocalAvoidanceOff()
	{
		agent.enabled = true;
		obs.enabled = false;
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
