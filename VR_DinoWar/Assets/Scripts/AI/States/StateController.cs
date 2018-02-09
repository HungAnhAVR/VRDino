using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class StateController : MonoBehaviour {

	public State currentState;
	public GameObject currentDestination;
	public State remainInState;
	public  bool AIEnabled;

	[HideInInspector] public float minimumRange = 2;
	[HideInInspector] public Player playerReference;
	[HideInInspector] public Enemy enemy; 	//enemy using this state controller

	// Use this for initialization
	void Start () {
		playerReference = Player.instance;
		enemy = transform.root.GetComponent<Enemy> ();
	
		GetDestination ();
	}
	
	// Update is called once per frame
	void Update () {
		if(AIEnabled)
		currentState.UpdateState (this);
		if(!playerReference)
			playerReference = Player.instance;
	}

	public void TransitionToState(State nextState)
	{
		if (nextState != remainInState) {
			currentState = nextState;
		}
	}

	public void GetDestination()
	{
		string tag = "Waypoint";
		
		GameObject[] waypointObjs = GameObject.FindGameObjectsWithTag (tag);
		int randomNo = Random.Range (0,waypointObjs.Length);

		currentDestination = waypointObjs[randomNo];
	}
}
