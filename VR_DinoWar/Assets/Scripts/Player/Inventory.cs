using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using VRTK;

public class Inventory : MonoBehaviour {

	public GameObject clubPrefab;
	public GameObject spearPrefab;
	public GameObject bowPrefab;
	public GameObject grenadePrefab;

	public Transform rightHand;
	public Transform leftHand;

	private VRTK_InteractTouch RightHandTouch;
	private VRTK_InteractGrab RightHandGrab;

	private VRTK_InteractTouch LeftHandTouch;
	private VRTK_InteractGrab LeftHandGrab;

	// Use this for initialization
	IEnumerator Start () {
		RightHandTouch = rightHand.GetComponent<VRTK_InteractTouch> ();
		RightHandGrab = rightHand.GetComponent<VRTK_InteractGrab> ();

		LeftHandTouch = leftHand.GetComponent<VRTK_InteractTouch> ();
		LeftHandGrab = leftHand.GetComponent<VRTK_InteractGrab> ();

		yield return new WaitForSeconds (1);
	}

	// Update is called once per frame
	void Update () {
		
	}

	public void GetClub(bool isRightHand = true)
	{
		Grab (isRightHand,clubPrefab);
	}

	public void GetSpear(bool isRightHand = true)
	{
		Grab (isRightHand,spearPrefab);
	}

	public void GetBow(bool isRightHand = true)
	{
		Grab (isRightHand,bowPrefab);
	}

	public void GetGrenade(bool isRightHand = true)
	{
		Grab (isRightHand,grenadePrefab);
	}

	void Grab(bool isRightHand,GameObject prefab)
	{
		print ("INVE");
		bool dropped = Drop (isRightHand,prefab);

		if (!dropped)
			return;

		GameObject weapon = Instantiate (prefab);

		if (isRightHand) {
			RightHandTouch.ForceTouch (weapon);
			RightHandGrab.AttemptGrab ();
		} else {
			LeftHandTouch.ForceTouch (weapon);
			LeftHandGrab.AttemptGrab ();
		}

	}

	bool Drop(bool isRightHand,GameObject prefab)
	{
		GameObject grabbedObj;
		if (isRightHand) {
			grabbedObj = RightHandGrab.GetGrabbedObject ();
			if (grabbedObj != null) {
				if (!isSameType (grabbedObj, prefab)) {
					RightHandGrab.ForceRelease ();
					return true;
				} else {
					print ("SAME TYPE");
					return false;
				}
			
			}
		} else {
			grabbedObj = LeftHandGrab.GetGrabbedObject ();
			if (grabbedObj != null) {
				if (!isSameType (grabbedObj, prefab)) {
					LeftHandGrab.ForceRelease ();
					return true;
				} else {
					print ("SAME TYPE");
					return false;
				}
			}
		}

		return true;
	}

	bool isSameType(GameObject weapongo1,GameObject weapongo2)
	{
		Weapon weapon1 = weapongo1.GetComponent<Weapon> ();
		Weapon weapon2 = weapongo2.GetComponent<Weapon> ();

		if (!weapon1 || !weapon2) {
			return false;
		}

		if (weapon1.GetType ().Equals (weapon2.GetType ())) {
			return true;
		}

		return false;
	}
}
