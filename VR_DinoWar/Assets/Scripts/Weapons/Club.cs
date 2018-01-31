using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using VRTK;

public class Club : Weapon {

	void Start()
	{
		Initialize ();
	}

	void Update()
	{

	}

	void FixedUpdate()
	{
		CalculateVelocity ();
	}

}