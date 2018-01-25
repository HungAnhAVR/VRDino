using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu (menuName ="PluggableAI/Actions/CloseIn")]
public class CloseIn : Action {

	public override void Act (StateController controller)
	{
		GoIn (controller);
	}

	private void GoIn(StateController controller)
	{
		if (controller.playerReference != null) {
			controller.enemy.agent.destination = controller.playerReference.transform.position;
			controller.enemy.agent.isStopped = false;
		} else {
			Debug.Log ("currentWaypoint is null!");
		}
	}
}
