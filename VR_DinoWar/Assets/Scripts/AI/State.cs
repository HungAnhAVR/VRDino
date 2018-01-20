using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class State : ScriptableObject {

	public Action[] actions;

	public void UpdateState(StateController controller)
	{
		DoActions (controller);
	}

	public void DoActions(StateController controller)
	{
		for (int i = 0; i < actions.Length; i++) {
			actions [i].Act (controller);
		}
	}
}
