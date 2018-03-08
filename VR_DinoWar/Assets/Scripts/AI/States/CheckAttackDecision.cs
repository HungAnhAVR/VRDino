using System.Collections;
using System.Collections.Generic;
using UnityEngine;
[CreateAssetMenu (menuName ="PluggableAI/Actions/CheckAttackDecision")]
public class CheckAttackDecision : Decision {

	public override bool Decide (StateController controller)
	{
		bool decide = CheckMaxAttkEnemy (controller);
		return decide;
	}

	bool CheckMaxAttkEnemy(StateController controller)
	{
		Debug.Log (controller.enemy.animState);
		if (controller.playerReference == null)
			return false;

		if (controller.playerReference.enemyNo < Utility.MAX_ATTK_ENEMY) {

			if(Player.instance.enemyNo < Utility.MAX_ATTK_ENEMY)
				Player.instance.enemyNo++;

			return true;
		} else {

			return false;
		}

	}
}
