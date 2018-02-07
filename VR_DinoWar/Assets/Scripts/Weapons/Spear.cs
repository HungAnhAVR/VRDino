using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using VRTK;

public class Spear : Weapon {
	
	protected override void Awake()
	{
		base.Awake ();
		InteractableObjectGrabbed += new InteractableObjectEventHandler(ResetPosition);
	}

	void Start()
	{
		Initialize ();
	}
	float num = 0;
	protected override void Update()
	{
		base.Update ();

		if (inFlight) {
			transform.eulerAngles += Vector3.one * 1f;
			transform.eulerAngles = new Vector3 (transform.eulerAngles.x,initialAngle.y,initialAngle.z);
		}

			
	}
	Vector3 vec ;

	protected override void FixedUpdate()
	{
		base.FixedUpdate ();
		CalculateVelocity ();
	}

	void SpearPhysics()
	{

	}

	protected override void OnHitSurface(Transform hitSurface)
	{
		base.OnHitSurface (hitSurface);	
		// Stop spear dead on its track
		interactableRigidbody.velocity = Vector3.zero;
		interactableRigidbody.isKinematic = true;		
		weaponCollider.enabled = false;
		transform.parent = hitSurface;
	}

}
