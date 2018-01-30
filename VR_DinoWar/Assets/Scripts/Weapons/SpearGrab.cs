using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using VRTK.GrabAttachMechanics;
using VRTK;

public class SpearGrab : VRTK_ChildOfControllerGrabAttach {

	Spear spear;

	// Use this for initialization
	void Start () {
		spear = this.GetComponent<Spear> ();
	}
	
	// Update is called once per frame
	void Update () {

	}

	protected override void ThrowReleasedObject(Rigidbody objectRigidbody)
	{
		if (grabbedObjectScript != null)
		{
			VRTK_ControllerReference controllerReference = VRTK_ControllerReference.GetControllerReference(grabbedObjectScript.GetGrabbingObject());
			if (VRTK_ControllerReference.IsValid(controllerReference) && controllerReference.scriptAlias != null)
			{
				VRTK_InteractGrab grabbingObjectScript = controllerReference.scriptAlias.GetComponent<VRTK_InteractGrab>();
				if (grabbingObjectScript != null)
				{
					Transform origin = VRTK_DeviceFinder.GetControllerOrigin(controllerReference);

					Vector3 velocity = VRTK_DeviceFinder.GetControllerVelocity(controllerReference);
					Vector3 angularVelocity = VRTK_DeviceFinder.GetControllerAngularVelocity(controllerReference);
					float grabbingObjectThrowMultiplier = grabbingObjectScript.throwMultiplier;

					if (origin != null)
					{
						objectRigidbody.velocity =  (objectRigidbody.transform.forward * throwMultiplier * origin.TransformVector(velocity).magnitude );

						if (spear != null) {
							spear.Thrown ();
						}
						else {
							Debug.Log ("NO SPEAR SCRIPT!");
						}
						print (origin.TransformVector(velocity).magnitude);
					//	objectRigidbody.velocity = origin.TransformVector(velocity) * (grabbingObjectThrowMultiplier * throwMultiplier);
						objectRigidbody.angularVelocity = origin.TransformDirection(angularVelocity);
					}
					else
					{
						objectRigidbody.velocity = velocity * (grabbingObjectThrowMultiplier * throwMultiplier);
						objectRigidbody.angularVelocity = angularVelocity;
					}

					if (throwVelocityWithAttachDistance)
					{
						Collider rigidbodyCollider = objectRigidbody.GetComponentInChildren<Collider>();
						if (rigidbodyCollider != null)
						{
							Vector3 collisionCenter = rigidbodyCollider.bounds.center;
							objectRigidbody.velocity = objectRigidbody.GetPointVelocity(collisionCenter + (collisionCenter - transform.position));
						}
						else
						{
							objectRigidbody.velocity = objectRigidbody.GetPointVelocity(objectRigidbody.position + (objectRigidbody.position - transform.position));
						}
					}
				}
			}
		}

	}
}
