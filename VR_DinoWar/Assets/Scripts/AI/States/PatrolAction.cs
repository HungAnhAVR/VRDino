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
		if (controller.enemy.agent.isStopped) {
			controller.enemy.agent.isStopped = false;
		}

		if (controller.enemy.agent.remainingDistance < controller.enemy.agent.stoppingDistance) {
			controller.enemy.agent.destination = GetRandomDestination ();
		} 

		controller.enemy.Walk ();
	}

	public Vector3 GetRandomDestination()
	{
		string tag = "Waypoint";

		GameObject[] waypointObjs = GameObject.FindGameObjectsWithTag (tag);
		int randomNo = Random.Range (0,waypointObjs.Length);

		return waypointObjs[randomNo].transform.position;
	}
}
