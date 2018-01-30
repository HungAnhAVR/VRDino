using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using VRTK;

public class Spear : VRTK_InteractableObject {
	
	private float impactMagnifier = 120f;
	private float collisionForce = 0f;
	private float maxCollisionForce = 4000f;
	private VRTK_ControllerReference controllerReference;

	public float force;
	Rigidbody rb;

	bool inFlight;
	Vector3 initialAngle;
	// Use this for initialization
	void Start () {
		rb = this.GetComponent<Rigidbody> ();
	}
	
	// Update is called once per frame
	void Update () {
		if (inFlight) {
			transform.eulerAngles -= Vector3.left * .5f;
			transform.eulerAngles = new Vector3 (transform.eulerAngles.x,initialAngle.y,initialAngle.z);
		}
	}

	public void Thrown()
	{
		inFlight = true;
		initialAngle = transform.eulerAngles;
	}

	public override void Grabbed(VRTK_InteractGrab grabbingObject)
	{
		base.Grabbed(grabbingObject);
		controllerReference = VRTK_ControllerReference.GetControllerReference(grabbingObject.controllerEvents.gameObject);
		ResetPosition ();
	}

	protected override void OnEnable()
	{
		base.OnEnable();
		controllerReference = null;
		interactableRigidbody.collisionDetectionMode = CollisionDetectionMode.Continuous;
	}

	public void ResetPosition()
	{
		print ("ResetPosition");
		transform.localPosition = Vector3.zero;
		transform.localEulerAngles = Vector3.zero;
	}

	private void OnHitSurface(Transform hitSurface)
	{
		print ("OnCollisionEnter");		
		rb.velocity = Vector3.zero;
		rb.isKinematic = true;		
		inFlight = false;
		transform.parent = hitSurface;
	}

	private void Reset()
	{
		rb.velocity = Vector3.zero;
		rb.isKinematic = false;
		inFlight = false;
	}

	private void OnCollisionEnter(Collision collision)
	{
			
		if (VRTK_ControllerReference.IsValid(controllerReference) && IsGrabbed())
		{
			collisionForce = VRTK_DeviceFinder.GetControllerVelocity(controllerReference).magnitude * impactMagnifier;
			var hapticStrength = collisionForce / maxCollisionForce;
			VRTK_ControllerHaptics.TriggerHapticPulse(controllerReference, hapticStrength, 0.5f, 0.01f);
			print ("collisionForce " + collisionForce + "  maxCollisionForce  " + maxCollisionForce);
		}
		else
		{
			collisionForce = collision.relativeVelocity.magnitude * impactMagnifier;
		}


		// This means that the spear is thrown and hit a surface
		if (inFlight) {
			OnHitSurface (collision.transform);
			inFlight = false;
		}

	}
}
