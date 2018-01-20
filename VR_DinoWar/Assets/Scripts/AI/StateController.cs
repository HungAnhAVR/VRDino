using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class StateController : MonoBehaviour {

	public State currentState;

	[HideInInspector] public NavMeshAgent agent;
	[HideInInspector] public Player playerReference;

	// Use this for initialization
	void Start () {
		playerReference = Player.instance;
		agent = GetComponent<NavMeshAgent> ();
	}
	
	// Update is called once per frame
	void Update () {
		currentState.UpdateState (this);
	}
}
