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

		// Jump and walk only applies to grounded enemies
		if (controller.enemy.isGrounded) {
			if (controller.enemy.agent.isOnOffMeshLink) {
				controller.enemy.Jump ();
			} else {
				controller.enemy.Walk ();
			}
		} else {
			// fix a speed bug when cross off mesh links
			if (controller.enemy.agent.isOnOffMeshLink) {
				controller.enemy.agent.speed = controller.enemy.initialSpeed / 3;
			} else {
				controller.enemy.agent.speed = controller.enemy.initialSpeed;
			}
		}

	}
		
}
