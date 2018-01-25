using System.Collections;
using System.Collections.Generic;
using UnityEngine;
[CreateAssetMenu (menuName ="PluggableAI/Actions/HitAndRunAction")]
public class HitAndRunAction : Action {

	public override void Act (StateController controller)
	{
		HitAndRun (controller);
	}

	private void HitAndRun(StateController controller)
	{
		controller.enemy.agent.destination = controller.playerReference.transform.position;

		float distanceToPlayer = Vector3.Distance(controller.transform.position,controller.enemy.agent.destination);

		if (distanceToPlayer < controller.enemy.agent.stoppingDistance) {
			controller.enemy.Attack ();
		}
	}
}
