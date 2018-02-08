using System.Collections;
using System.Collections.Generic;
using UnityEngine;
[CreateAssetMenu (menuName ="PluggableAI/Actions/CheckPlayerInRadiusDecision")]
public class CheckPlayerInRadiusDecision : Decision {

	public override bool Decide (StateController controller)
	{
		bool decide = CheckAttack (controller);
		return decide;
	}

	bool CheckAttack(StateController controller)
	{
		float distanceToPlayer = Vector3.Distance (controller.enemy.body.transform.position,controller.playerReference.transform.position);

		if (distanceToPlayer < controller.enemy.agent.stoppingDistance + 1) {
			return true;
		} else {
			return false;
		}

	}
}
