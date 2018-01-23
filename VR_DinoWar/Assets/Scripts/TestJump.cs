using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class TestJump : MonoBehaviour {

	public NavMeshAgent agent;
	public Transform destination;

	void Start () {
		agent.destination = destination.position;
	
	}

	void Update()
	{
		if(agent.isOnOffMeshLink)
		agent.speed = 1;
	}
}
