using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Utility {

	private static Utility instance;

	private Utility() {}

	public static Utility Instance
	{
		get 
		{
			if (instance == null)
			{
				instance = new Utility();
			}
			return instance;
		}
	}

	public const int MAX_ATTK_ENEMY = 4;
}
