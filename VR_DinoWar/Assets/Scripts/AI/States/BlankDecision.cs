using System.Collections;
using System.Collections.Generic;
using UnityEngine;
[CreateAssetMenu (menuName ="PluggableAI/Actions/BlankDecision")]
public class BlankDecision : Decision {

	public override bool Decide (StateController controller)
	{
		return true;
	}

}
