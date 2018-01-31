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

}
