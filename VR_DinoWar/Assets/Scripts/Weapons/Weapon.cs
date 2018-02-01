using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using VRTK;

public class Weapon : VRTK_InteractableObject {

	// Reference to the controller holding this weapon
	private VRTK_ControllerReference controllerReference;
	// is the weapon being thrown?
	protected bool inFlight;
	// initial angle before the weapon is thrown
	private Vector3 initialAngle;
	// local collider reference 
	public BoxCollider weaponCollider;
	// melee collider size - for melee uses only
	public Vector3 meleeScale;
	// thrown collider size - for when weapon is thrown 
	public Vector3 thrownScale;
	//Mainly use for calculating weapon velocity
	public Transform weaponTip;
	// Min force player must physically applied to have a valid hit
	public float minForce;

	// for weapon velocity calculating
	private Vector3 previous;
	private float velocity;

	// Cache
	private Enemy enemy;

	// Use this for initialization
	protected void Initialize () {
		weaponCollider.isTrigger = true;
	}

	protected void Loop () {
		// Physics hack
		if (inFlight) {
			transform.eulerAngles += new Vector3( 1 * .75f , 0 , 0 );
			transform.eulerAngles = new Vector3 (transform.eulerAngles.x, initialAngle.y, initialAngle.z);
		} 
	}

	// Put this in FixedUpdate()
	protected void CalculateVelocity()
	{
		velocity = ((weaponTip.position - previous).magnitude) / Time.deltaTime;
		previous = weaponTip.position;
	}

	// when weapon is thrown
	public void Thrown()
	{
		inFlight = true;
		initialAngle = transform.eulerAngles;

		weaponCollider.isTrigger = false;
		weaponCollider.size = thrownScale;
	}

	// reset position on grab
	public override void Grabbed(VRTK_InteractGrab grabbingObject)
	{
		base.Grabbed(grabbingObject);
		controllerReference = VRTK_ControllerReference.GetControllerReference(grabbingObject.controllerEvents.gameObject);

		weaponCollider.size = meleeScale;
		interactableRigidbody.isKinematic = false;
	}

	protected void ResetPosition(object sender, InteractableObjectEventArgs e)
	{
		print ("ResetPosition");
		StartCoroutine (ResetPosition_async());
	}

	IEnumerator ResetPosition_async()
	{
		yield return new WaitForEndOfFrame();
		transform.localPosition = Vector3.zero;
		transform.localEulerAngles = Vector3.zero;
	}

	// On hit something when thrown
	protected virtual void OnHitSurface(Transform hitSurface)
	{
		print ("OnHitSurface" + hitSurface.name);		
		inFlight = false;
	}

	// This is used for throwing 
	private void OnCollisionEnter(Collision collision)
	{
		// This means that the weapon is thrown and hit a surface
		if (inFlight) {
			OnHitSurface (collision.transform);
			CheckIfEnemyAndDealDamage (collision.transform);
			inFlight = false;
		}
	}

	// use for melee
	private void OnTriggerEnter(Collider collision)
	{
//		print ("spearVelocity " +  velocity + " collision " + collision.transform.name);
		if (VRTK_ControllerReference.IsValid(controllerReference) && IsGrabbed())
		{
			//Only applies damage if player physically put some force to weapon
			if ( velocity > minForce) {
				CheckIfEnemyAndDealDamage (collision.transform);
			}
		}
	}

	void CheckIfEnemyAndDealDamage(Transform t)
	{
		enemy = t.root.GetComponent<Enemy> ();
		// If player indeed hit the enemy
		if (enemy != null) {
			enemy.Hit ();
		}
	}




}
