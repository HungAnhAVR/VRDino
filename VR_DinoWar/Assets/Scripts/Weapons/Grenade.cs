using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using VRTK;

public class Grenade : Weapon {
	
	public Collider blastRadius;

	public float fuseTime = 3;

	float timeCount = 0;

	public GameObject explosionPrefab;

	// Use this for initialization
	void Start () {
		Initialize ();
		blastRadius.enabled = false;
		Thrown ();
	}
	
	// when weapon is thrown
	public override void Thrown(bool enoughForce = true)
	{
		base.Thrown ();
		StartCoroutine (Explode());
	}

	void OnTriggerEnter(Collider other) {
		print ("BOOM");
		CheckIfEnemyAndBlast (other.transform);
		timeCount += Time.deltaTime;
	}

	protected void CheckIfEnemyAndBlast(Transform t)
	{
		enemy = t.root.GetComponent<Enemy> ();
		// If player indeed hit the enemy
		if (enemy != null) {
			enemy.Blast (transform.position);
		}

		Rigidbody rb = t.GetComponent<Rigidbody> ();
		if (rb != null) {
			rb.AddExplosionForce (1000,transform.position,100);
		}
	}

	IEnumerator Explode()
	{
		yield return new WaitForSeconds (fuseTime);
		Instantiate (explosionPrefab,transform.position,transform.rotation);
		blastRadius.enabled = true;
		yield return new WaitForSeconds (.05f);
		Destroy (gameObject);
	}
		
		
}
