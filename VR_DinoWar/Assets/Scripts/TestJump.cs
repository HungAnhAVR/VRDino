using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class TestJump : MonoBehaviour {


	void Start () {
		//iTween.MoveTo (gameObject, iTween.Hash ("path", iTweenPath.GetPath ("path"), "time", 40, "easetype",iTween.EaseType.linear, "orienttopath", true));
		iTween.MoveTo(gameObject, 
			iTween.Hash("path", iTweenPath.GetPath("path1"), 
				"orienttopath", true, 
				"looktime", 0.001f, 
				"lookahead", 0.001f, 
				"speed", 90, "easetype", 
				iTween.EaseType.linear, 
				"oncomplete", "onCameraShakeComplete"));
	}

	void onCameraShakeComplete()
	{
		iTween.MoveTo(gameObject, 
			iTween.Hash("path", iTweenPath.GetPath("path1"), 
				"orienttopath", true, 
				"looktime", 0.001f, 
				"lookahead", 0.001f, 
				"speed", 90, "easetype", 
				iTween.EaseType.linear, 
				"oncomplete", "onCameraShakeComplete"));
	}
}
