using System.Collections;
using System.Collections.Generic;
using UnityEngine;
[CreateAssetMenu (menuName ="PluggableAI/Actions/CloseInDecision")]
public class CloseInDecision : Decision {

	public override bool Decide (StateController controller)
	{
		bool decide = CheckMaxAttkEnemy (controller);
		return decide;
	}

	bool CheckMaxAttkEnemy(StateController controller)
	{

		if (controller.playerReference.enemyNo < Utility.MAX_ATTK_ENEMY) {

			if(Player.instance.enemyNo < Utility.MAX_ATTK_ENEMY)
				Player.instance.enemyNo++;
			
			return true;
		} else {

			return false;
		}

	}
}
