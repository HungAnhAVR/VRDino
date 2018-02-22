using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using VRTK;

public class Spear : Weapon {

	public Collider spearTipCollider;
	public bool enoughForce;
	
	protected override void Awake()
	{
		base.Awake ();
		InteractableObjectGrabbed += new InteractableObjectEventHandler(ResetPosition);
	}

	void Start()
	{
		Initialize ();
	}

	float angle;

	protected override void Update()
	{
		base.Update ();

		if (inFlight) {

			angle = transform.eulerAngles.x;
			angle += Time.deltaTime * 40;
			transform.eulerAngles = new Vector3 (angle, initialAngle.y, initialAngle.z);

		}
		//transform.eulerAngles = new Vector3 (transform.eulerAngles.x, transform.eulerAngles.y + 6f, transform.eulerAngles.z);
		//if(!hasHitSurface)
		//print (interactableRigidbody.velocity.magnitude);

	}

	public override void Thrown(bool enoughForce = true)
	{
		base.Thrown ();
		this.enoughForce = enoughForce;
	}

	protected override void FixedUpdate()
	{
		base.FixedUpdate ();
		CalculateVelocity ();
	}

	void SpearPhysics()
	{

	}

	protected override void OnHitSurface(Transform hitSurface)
	{
		base.OnHitSurface (hitSurface);	

		if (enoughForce) {
			weaponCollider.enabled = false;
			interactableRigidbody.isKinematic = true;	
		} else {

		}

		Enemy enemy = hitSurface.root.GetComponent<Enemy> ();
		if (enemy != null) {
			transform.parent = hitSurface.transform;
			weaponCollider.enabled = false;
			interactableRigidbody.isKinematic = true;	
		}

		hasHitSurface = true;
	}
		
	bool hasHitSurface;

}
