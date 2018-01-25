using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class StateController : MonoBehaviour {

	public State currentState;
	public GameObject currentDestination;
	public State remainInState;


	[HideInInspector] public Player playerReference;
	[HideInInspector] public Enemy enemy; 	//enemy using this state controller

	// Use this for initialization
	void Start () {
		playerReference = Player.instance;
		enemy = GetComponent<Enemy> ();

		GetDestination ();
	}
	
	// Update is called once per frame
	void Update () {
		currentState.UpdateState (this);
	}

	public void TransitionToState(State nextState)
	{
		if (nextState != remainInState) {
			currentState = nextState;
		}
		enemy.hasAttacked = false;
	}

	public void GetDestination()
	{
		string tag = "Waypoint";

		if(enemy.canFly)
			tag = "SkyWaypoint";
		
		GameObject[] waypointObjs = GameObject.FindGameObjectsWithTag (tag);
		int randomNo = Random.Range (0,waypointObjs.Length);

		currentDestination = waypointObjs[randomNo];
	}
}
