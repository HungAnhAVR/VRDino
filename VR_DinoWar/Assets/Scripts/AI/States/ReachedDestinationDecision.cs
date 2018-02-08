using System.Collections;
using System.Collections.Generic;
using UnityEngine;
[CreateAssetMenu (menuName ="PluggableAI/Actions/ReachedDestinationDecision")]
public class ReachedDestinationDecision : Decision {

	public override bool Decide (StateController controller)
	{
		bool check = CheckDestination (controller);
		return check;
	}

	private bool CheckDestination(StateController controller)
	{
		float distanceFromDestination = Vector3.Distance (controller.transform.position,controller.enemy.agent.destination);

		if (distanceFromDestination < controller.enemy.agent.stoppingDistance) {
			return true;
		} else {
			return false;
		}


	}
}
