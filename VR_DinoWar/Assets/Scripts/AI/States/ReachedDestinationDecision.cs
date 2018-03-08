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
	float distance;
	private bool CheckDestination(StateController controller)
	{
		distance = Vector3.Distance(controller.enemy.transform.position,controller.enemy.agent.destination);
		if (distance < controller.enemy.agent.stoppingDistance) {
			return true;
		} else {
			return false;
		}
	}
}
