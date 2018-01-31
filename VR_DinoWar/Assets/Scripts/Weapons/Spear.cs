using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using VRTK;

public class Spear : Weapon {
	
	void Start()
	{
		Initialize ();
	}

	void Update()
	{
		Loop ();
	}

	void FixedUpdate()
	{
		CalculateVelocity ();
	}

	protected override void OnHitSurface(Transform hitSurface)
	{
		base.OnHitSurface (hitSurface);	
		// Stop spear dead on its track
		rb.velocity = Vector3.zero;
		rb.isKinematic = true;		
		transform.parent = hitSurface;
	}

}
