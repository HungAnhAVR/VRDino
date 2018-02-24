using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using VRTK;

public class Club : Weapon {

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
		CalculateVelocity ();
	}
		
	protected override void FixedUpdate()
	{
		base.FixedUpdate ();
	}

}