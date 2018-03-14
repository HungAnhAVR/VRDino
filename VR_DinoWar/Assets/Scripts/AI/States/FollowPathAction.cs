using System.Collections;
using System.Collections.Generic;
using UnityEngine;
[CreateAssetMenu (menuName ="PluggableAI/Actions/FollowPathAction")]
public class FollowPathAction : Action {
	
	public override void Init (StateController controller)
	{

	}

	public override void Act (StateController controller)
	{
		FollowPath (controller);
	}
	
	private void FollowPath(StateController controller)
	{
		
	}

}
