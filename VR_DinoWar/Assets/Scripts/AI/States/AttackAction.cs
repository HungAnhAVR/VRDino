using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu (menuName ="PluggableAI/Actions/AttackAction")]
public class AttackAction : Action {

	public override void Init (StateController controller)
	{
		controller.enemy.animator.SetInteger ("State", 9);
		controller.enemy.onEndAttackAnim += EndAttackAnim;

	}

	public override void Act (StateController controller)
	{
		Attack (controller);
	}

	private void Attack(StateController controller)
	{
		float distanceToPlayer = Vector3.Distance(controller.transform.position,controller.playerReference.transform.position);

		if (distanceToPlayer <= 2) {
			controller.enemy.animator.SetInteger ("State", 9);
			controller.enemy.FaceTarget (controller.playerReference.transform.position);
			controller.enemy.LocalAvoidanceOn ();
		} else {
			controller.enemy.LocalAvoidanceOff ();
		}

	}

	private void EndAttackAnim(StateController controller)
	{
		Debug.Log ("HIT ME");
	}
		
}
