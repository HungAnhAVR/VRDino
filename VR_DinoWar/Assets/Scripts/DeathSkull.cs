using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DeathSkull : Cacheable {

	float duration = 2.5f;
	float timeCount = 0;

	ParticleSystem[] particles;

	// Use this for initialization
	void Start () {
		particles = this.GetComponentsInChildren<ParticleSystem> ();
	}
	
	// Update is called once per frame
	void Update () {

		if (_living) {

			timeCount += Time.deltaTime;

			if (timeCount > duration) {
				timeCount = 0;
				Destroy ();
			}

		}
		
	}

	public override void OnDestroy ()
	{
		foreach (ParticleSystem particle in particles) {
			particle.Stop ();
		}
	}

	public override void OnLive ()
	{
		particles = this.GetComponentsInChildren<ParticleSystem> ();
		foreach (ParticleSystem particle in particles) {
			particle.Play ();
		}
	}


}
