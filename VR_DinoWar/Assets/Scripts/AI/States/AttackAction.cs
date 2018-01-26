using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu (menuName ="PluggableAI/Actions/AttackAction")]
public class AttackAction : Action {

	public override void Act (StateController controller)
	{
		Attack (controller);
	}

	private void Attack(StateController controller)
	{
		if(controller.enemy.agent.enabled)
		controller.enemy.agent.destination = controller.playerReference.transform.position;

		float distanceToPlayer = Vector3.Distance(controller.transform.position,controller.enemy.agent.destination);

		if (distanceToPlayer < controller.enemy.agent.stoppingDistance) {
			controller.enemy.Attack ();
		}
	}
		
}
