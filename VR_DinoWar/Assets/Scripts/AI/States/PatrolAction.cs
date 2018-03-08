using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu (menuName ="PluggableAI/Actions/PatrolAction")]
public class PatrolAction : Action {

	public override void Act (StateController controller)
	{
		Patrol (controller);
	}

	private void Patrol(StateController controller)
	{
		controller.enemy.Walk ();

		if (!controller.enemy.agent.enabled) {
			return;
		}

		if (controller.enemy.agent.isStopped) {
			controller.enemy.agent.isStopped = false;
		}	
		//init
		if (controller.enemy.agent.remainingDistance == 0) {
			controller.enemy.agent.destination = GetRandomDestination ();
		}
	}

	public Vector3 GetRandomDestination()
	{
		string tag = "Waypoint";

		GameObject[] waypointObjs = GameObject.FindGameObjectsWithTag (tag);
		int randomNo = Random.Range (0,waypointObjs.Length);

		return waypointObjs[randomNo].transform.position;
	}
}
