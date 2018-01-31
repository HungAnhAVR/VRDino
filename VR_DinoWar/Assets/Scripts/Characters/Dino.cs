using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class Dino : Enemy {

	// Use this for initialization
	void Start () {
		Initialize ();
	}
	
	// Update is called once per frame
	void Update () {
		Loop ();
	}

	private void OnCollisionEnter(Collision collision)
	{
		print ("IM HIT   "   + collision.transform.name);

	}

}
