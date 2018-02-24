using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//ATTACH TO ObjectPool gameObject

public class ObjectPool : MonoBehaviour {

	public static ObjectPool instance;

	GenericObject<HitNumber> hitNumbers;

	void Awake()
	{
		instance = this;
	}

	// Use this for initialization
	void Start () {
		hitNumbers = new GenericObject<HitNumber>(ObjectFactory.PrefabType.HitNumber,10);
	}

	public HitNumber GetHitNumber()
	{
		return hitNumbers.GetObj ();
	}
		
}
