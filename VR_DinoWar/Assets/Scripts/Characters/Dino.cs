using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using RootMotion.Dynamics;

public class Dino : Enemy {


	// Use this for initialization
	IEnumerator Start () {
		Initialize ();
		Rigidbody[] rbs = this.GetComponentsInChildren<Rigidbody> ();
		foreach(Rigidbody rb in rbs)
		{
			rb.useGravity = false;
			rb.isKinematic = false;
		}
		yield return new WaitForSeconds (1);
	}
		
	// Update is called once per frame
	void Update () {
		Loop ();

		if(Input.GetKeyDown(KeyCode.P))
			{
			Stun ();
			}
	}

	private void OnCollisionEnter(Collision collision)
	{

	}

	public Rigidbody pelvis;
	protected override void ApplyPhysics()
	{
		Rigidbody[] rbs = this.GetComponentsInChildren<Rigidbody> ();
		foreach(Rigidbody rb in rbs)
		{
			rb.useGravity = true;
		}
		bodyIK.enabled = false;
		animator.enabled = false;
	}



}
