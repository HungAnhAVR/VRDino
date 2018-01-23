using System.Collections;
using System.Collections.Generic;
using UnityEngine;
[CreateAssetMenu (menuName ="PluggableAI/Actions/CheckAttackDecision")]
public class CheckAttackDecision : Decision {

	public override bool Decide (StateController controller)
	{
		bool decide = CheckAttack (controller);
		return decide;
	}

	bool CheckAttack(StateController controller)
	{
		return true;
	}
}
