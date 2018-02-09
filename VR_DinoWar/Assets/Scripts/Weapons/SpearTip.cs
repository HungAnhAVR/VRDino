using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpearTip : MonoBehaviour {

	Collider collider;

	void Start()
	{
		collider = this.GetComponent<Collider> ();
	}

	void OnTriggerEnter(Collider collision)
	{
		print ("AAAAAAAA");
	}
}
