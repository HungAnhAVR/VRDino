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
		float distanceToPlayer = Vector3.Distance(controller.transform.position,controller.playerReference.transform.position);

		if (distanceToPlayer <= 2) {
			controller.enemy.Attack ();
		} else {
			controller.enemy.Steer ();
		}

	}
		
}
