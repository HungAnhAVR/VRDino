using System.Collections;
using System.Collections.Generic;
using UnityEngine;
[CreateAssetMenu (menuName ="PluggableAI/Actions/IdleAction")]
public class IdleAction : Action {
	
	public override void Init (StateController controller)
	{
		controller.enemy.animator.SetInteger ("State", 0);
		controller.enemy.animator.SetTrigger ("Idle Anim 1");
		controller.enemy.isIdleDone = false;
		controller.enemy.LocalAvoidanceOn ();

		controller.enemy.onIdleAnimDone += DoneIdle;
	}

	public override void Act (StateController controller)
	{
		Idling (controller);
	}

	private void Idling(StateController controller)
	{

	}

	private void DoneIdle(StateController controller)
	{
		controller.enemy.onIdleAnimDone -= DoneIdle;
		controller.enemy.LocalAvoidanceOff ();
	}
}
