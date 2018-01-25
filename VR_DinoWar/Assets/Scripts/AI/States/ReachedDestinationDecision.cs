using System.Collections;
using System.Collections.Generic;
using UnityEngine;
[CreateAssetMenu (menuName ="PluggableAI/Actions/ReachedDestinationDecision")]
public class ReachedDestinationDecision : Decision {

	public override bool Decide (StateController controller)
	{
		bool foundWaypoint = CheckDestination (controller);
		return foundWaypoint;
	}

	private bool CheckDestination(StateController controller)
	{
		float distanceFromDestination = Vector3.Distance (controller.transform.position,controller.enemy.agent.destination);

		if (distanceFromDestination < controller.enemy.agent.stoppingDistance) {
			controller.GetDestination (); //Reset destionation
			return true;
		} else {
			return false;
		}

	}
}
