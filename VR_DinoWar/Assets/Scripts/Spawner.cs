using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Spawner : MonoBehaviour {
	public GameObject prefab;
	// Use this for initialization

	IEnumerator Start () {

		while (true) {

			Instantiate (prefab,transform.position,transform.rotation);
			yield return new WaitForSeconds (Random.Range(12,20));
		}


	}
	

}
