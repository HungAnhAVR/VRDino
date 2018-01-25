using System.Collections;
using System.Collections.Generic;
using UnityEngine;
[CreateAssetMenu (menuName ="PluggableAI/Actions/HitAndRunDecision")]
public class HitAndRunDecision : Decision {

	public override bool Decide (StateController controller)
	{
		bool decide = CheckAttack (controller);
		return decide;
	}


	bool CheckAttack(StateController controller)
	{
		if (controller.enemy.hasAttacked) {
			return true;
		} else {
			return false;
		}

	}
}
