using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class TestAI : MonoBehaviour {

	public Transform target;
	NavMeshAgent agent;

	// Use this for initialization
	void Start () {
		agent = GetComponent<NavMeshAgent> ();
		agent.SetDestination (target.position);
	}
	
	// Update is called once per frame
	void Update () {
		
	}
}
