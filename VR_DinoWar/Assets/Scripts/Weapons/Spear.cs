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

	protected override void Update()
	{
		base.Update ();
		Loop ();
	}

	protected override void FixedUpdate()
	{
		base.FixedUpdate ();
		CalculateVelocity ();
	}

	protected override void OnHitSurface(Transform hitSurface)
	{
		base.OnHitSurface (hitSurface);	
		// Stop spear dead on its track
		interactableRigidbody.velocity = Vector3.zero;
		interactableRigidbody.isKinematic = true;		
		transform.parent = hitSurface;
	}

}
