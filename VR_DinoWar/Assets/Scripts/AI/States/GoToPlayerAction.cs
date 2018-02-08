using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu (menuName ="PluggableAI/Actions/GoToPlayerAction")]
public class GoToPlayerAction : Action {

	public override void Act (StateController controller)
	{
		GoToPlayer (controller);
	}

	private void GoToPlayer(StateController controller)
	{
		controller.enemy.agent.destination = controller.playerReference.transform.position;

		controller.enemy.Walk ();

	}
}
