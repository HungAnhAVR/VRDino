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
//		Debug.Log (controller.enemy.agent.remainingDistance);

		if (controller.enemy.agent.remainingDistance < controller.enemy.agent.stoppingDistance) {
			controller.enemy.Attack ();
		}
	
	}
		
}
