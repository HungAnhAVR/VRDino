using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : Character {

	public int enemyNo = 0;

	public static Player instance;

	void Awake()
	{
		instance = this;
	}

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}

}
