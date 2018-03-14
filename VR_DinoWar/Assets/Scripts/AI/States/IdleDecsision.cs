using System.Collections;
using System.Collections.Generic;
using UnityEngine;
[CreateAssetMenu (menuName ="PluggableAI/Actions/IdleDecision")]
public class IdleDecsision : Decision {

	public override bool Decide (StateController controller)
	{
		bool decide = CheckIdle (controller);
		return decide;
	}

	bool CheckIdle(StateController controller)
	{
		if (controller.enemy.isIdleDone)
			return true;
		else
			return false;

	}
}
