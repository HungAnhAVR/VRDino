using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Character : MonoBehaviour {

	public int hitPoints;

	public void OnHit(int dmg)
	{
		hitPoints -= dmg;

		if (hitPoints <= 0) {
			Die ();
		}
	}

	protected virtual void Die()
	{

	}

}
