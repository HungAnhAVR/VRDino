using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using VRTK;

public class Spear : VRTK_InteractableObject {
	
	private float impactMagnifier = 5;
	private float collisionForce = 0f;
	private float maxCollisionForce = 500f;
	private VRTK_ControllerReference controllerReference;

	private bool inFlight;
	private Vector3 initialAngle;

	private Rigidbody rb;

	public Collider meleeCollider;
	public Collider thrownCollider;

	//Mainly use for calculating velocity
	public Transform spearTip;

	// Use this for initialization
	void Start () {
		rb = this.GetComponent<Rigidbody> ();
	}


	// Update is called once per frame
	void Update () {
		if (inFlight) {
			transform.eulerAngles -= Vector3.left * .25f;
			transform.eulerAngles = new Vector3 (transform.eulerAngles.x, initialAngle.y, initialAngle.z);
			meleeCollider.enabled = false;
			thrownCollider.enabled = true;
		} else {
			meleeCollider.enabled = true;
			thrownCollider.enabled = false;
		}
	}

	Vector3 previous;
	float velocity;

	void FixedUpdate()
	{
		velocity = ((spearTip.position - previous).magnitude) / Time.deltaTime;
		previous = spearTip.position;

	//	print ("spearVelocity " +  velocity );
	}

	public void Thrown()
	{
		inFlight = true;
		initialAngle = transform.eulerAngles;
	}

	public override void Grabbed(VRTK_InteractGrab grabbingObject)
	{
		base.Grabbed(grabbingObject);
		controllerReference = VRTK_ControllerReference.GetControllerReference(grabbingObject.controllerEvents.gameObject);
		ResetPosition ();
	}

	protected override void OnEnable()
	{
		base.OnEnable();
		controllerReference = null;
		interactableRigidbody.collisionDetectionMode = CollisionDetectionMode.Continuous;
	}

	public void ResetPosition()
	{
		transform.localPosition = Vector3.zero;
		transform.localEulerAngles = Vector3.zero;
	}

	private void OnHitSurface(Transform hitSurface)
	{
		print ("OnHitSurface" + hitSurface.name);		
		rb.velocity = Vector3.zero;
		rb.isKinematic = true;		
		inFlight = false;
		transform.parent = hitSurface;
	}

	private void Reset()
	{
		rb.velocity = Vector3.zero;
		rb.isKinematic = false;
		inFlight = false;
	}

	Enemy enemy;
	// This is used for throwing spear
	private void OnCollisionEnter(Collision collision)
	{
		// This means that the spear is thrown and hit a surface
		if (inFlight) {
			OnHitSurface (collision.transform);
			inFlight = false;

			enemy = collision.transform.root.GetComponent<Enemy> ();
			// If player indeed hit the enemy
			if (enemy != null) {
				enemy.Hit ();
			}
		}
	}

	public float minForce;
	// use for melee
	private void OnTriggerEnter(Collider collision)
	{
		print ("spearVelocity " +  velocity + " collision " + collision.transform.name);
		if (VRTK_ControllerReference.IsValid(controllerReference) && IsGrabbed())
		{
			//Only applies damage if player physically put some force to weapon
			if ( velocity> minForce) {
				enemy = collision.transform.root.GetComponent<Enemy> ();
				// If player indeed hit the enemy
				if (enemy != null) {
					enemy.Hit ();
				}
			}
				
		}
	}

}
