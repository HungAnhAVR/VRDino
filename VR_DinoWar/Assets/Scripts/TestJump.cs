using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class TestJump : MonoBehaviour {

	public NavMeshAgent agent;
	public Transform destination;
	public Rigidbody rb;
	public Animator anim;
	IEnumerator Start()
	{
		yield return new WaitForSeconds(1);

		//Vector3 vel = destination.position - rb.transform.position;
		//rb.AddForceAtPosition (vel * 10000, rb.transform.position);

		rb.transform.position += Vector3.right * .5f;


	}

}
