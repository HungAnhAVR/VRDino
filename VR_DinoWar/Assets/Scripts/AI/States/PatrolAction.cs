using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu (menuName ="PluggableAI/Actions/PatrolAction")]
public class PatrolAction : Action {

	public override void Init (StateController controller)
	{
		controller.enemy.agent.destination = GetRandomDestination ();
		controller.enemy.animator.SetInteger ("State", 1);
		controller.enemy.LocalAvoidanceOff ();
	}

	public override void Act (StateController controller)
	{
		Patrol (controller);
	}

	private void Patrol(StateController controller)
	{

		if (!controller.enemy.agent.enabled) {
			return;
		}

		if (controller.enemy.agent.isStopped) {
			controller.enemy.agent.isStopped = false;
		}	

	}

	private Vector3 GetRandomDestination()
	{
		string tag = "Waypoint";

		GameObject[] waypointObjs = GameObject.FindGameObjectsWithTag (tag);
		int randomNo = Random.Range (0,waypointObjs.Length);

		return waypointObjs[randomNo].transform.position;
	}
}
