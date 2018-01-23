using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu (menuName ="PluggableAI/Actions/Engage")]
public class EngageAction : Action {

	public override void Act (StateController controller)
	{
		Engage (controller);
	}

	private void Engage(StateController controller)
	{
		if (controller.currentDestination != null) {
			controller.enemy.agent.destination = controller.currentDestination.transform.position;
			controller.enemy.agent.isStopped = false;
		} else {
			Debug.Log ("currentWaypoint is null!");
		}

		if (controller.enemy.agent.isOnOffMeshLink) {
			controller.enemy.Jump ();
		} else {
			controller.enemy.Walk ();
		}
	}
		
}
