using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ParticleLifetime : MonoBehaviour {

	public float time;
	float timeCount;

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		timeCount += Time.deltaTime;

		if (timeCount > time) {
			Destroy (gameObject);
		}
	}
}
