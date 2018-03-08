using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TextHitRandom : Cacheable {
	
	Transform playerTransform;
	float dist = 0;
	float timeCount = 0;
	void Update()
	{
		if (!_living)
			return;
		
		if (playerTransform == null) {
			if (Player.instance != null)
				playerTransform = Player.instance.transform;
		} else {
			transform.LookAt (playerTransform);
			transform.position += transform.forward * .05f;

			// Scale floating text to keep it consistent in distances
			dist = Vector3.Distance (playerTransform.position,transform.position);

			timeCount += Time.deltaTime;

			if (timeCount > 1) {
				timeCount = 0;
				Destroy ();
			}
		}
	}

	public override void OnDestroy ()
	{

	}

	ParticleSystem[] particles;

	public override void OnLive ()
	{
		timeCount = 0;

		if(particles == null)
		particles = this.GetComponentsInChildren<ParticleSystem> ();
		
		foreach (ParticleSystem particle in particles) {
			particle.Play ();
		}
	}
}
