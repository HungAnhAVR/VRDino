using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu (menuName ="PluggableAI/Actions/State")] 
public class State : ScriptableObject {

	public Action[] actions;
	public Transition[] transitions;

	public void UpdateState(StateController controller)
	{
		DoActions (controller);
		CheckTransition(controller);
	}

	public void DoActions(StateController controller)
	{
		for (int i = 0; i < actions.Length; i++) {
			actions [i].Act (controller);
		}
	}

	public void InitState(StateController controller)
	{
		for (int i = 0; i < actions.Length; i++) {
			actions [i].Init (controller);
		}
	}

	private void CheckTransition(StateController controller)
	{
		for (int i = 0; i < transitions.Length; i++) {
			bool decisionSucceeded = transitions [i].decision.Decide (controller);

			if (decisionSucceeded) {
				controller.TransitionToState (transitions [i].trueState);
			} else {
				controller.TransitionToState (transitions[i].falseState);
			}
		}
	}


}
