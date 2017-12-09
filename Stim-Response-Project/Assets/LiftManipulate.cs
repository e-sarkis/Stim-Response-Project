using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LiftManipulate : MonoBehaviour 
{
	public float initialLiftRange = 1.5f;
	public float throwForce = 200f;

	public float minLiftDist = 0.8f;
	public float startLiftDist = 1;
	public float maxLiftDist = 2.0f;
	private float currentLiftDist = 0.0f;

	GameObject objectLifted;
	Rigidbody rbLifted;
	Collider colliderLifted;
	
	// Update is called once per frame
	void Update () 
	{
		if (!objectLifted) ObtainLiftable(); 		// Check for Liftable / Obtain Liftable
		else
		{
			UpdateLiftedObject();	// Perform Updates on Current Liftable
			if (Input.GetKeyDown(KeyCode.Mouse0)) ThrowLiftedObject();
			if (Input.GetKeyDown(KeyCode.Mouse1)) ThrowLiftedObject(throwForce);
		} 
	}

	Quaternion rotationOffset;
    void UpdateLiftedObject()
	{
		objectLifted.transform.position = transform.position + (transform.forward * currentLiftDist);

		if (Input.GetButton("Use"))
		{
			float mX = Input.GetAxis("Mouse X");
			float mY = Input.GetAxis("Mouse Y");
			rotationOffset.eulerAngles += Quaternion.Euler(mY, -mX, 0).eulerAngles;
		}

		objectLifted.transform.rotation = Quaternion.Euler(transform.rotation.eulerAngles + rotationOffset.eulerAngles);

	}


	// Check for Input, and Grab the GameObject if in range and liftable
	void ObtainLiftable()
	{
		if (Input.GetKeyDown(KeyCode.Mouse0))
		{
			// We can attempt to lift object
			Debug.DrawRay(transform.position, transform.forward, Color.red, initialLiftRange);
			RaycastHit hit;
			if (Physics.Raycast(transform.position, transform.forward, out hit, initialLiftRange))
			{
				objectLifted = hit.transform.gameObject; // Obtained the GameObject
				PrepForManipulation();
			}
		}
	}

	void PrepForManipulation()
	{
		PhysicsProperties ppLifted = objectLifted.GetComponent<PhysicsProperties>();
		if (!ppLifted || !ppLifted.liftable)
		{
			objectLifted = null;
			return;
		}
		// The GameObject is a confirmed liftable, continue
		rbLifted = objectLifted.GetComponent<Rigidbody>();
		rbLifted.velocity = Vector3.zero;
		rbLifted.angularVelocity = Vector3.zero;
		rbLifted.useGravity = false;
		colliderLifted = objectLifted.GetComponent<Collider>();
		colliderLifted.enabled = false;
		currentLiftDist = startLiftDist;
		objectLifted.transform.position = transform.position + (transform.forward * currentLiftDist);
		rotationOffset = new Quaternion();
	}

	void ThrowLiftedObject(float force = 0f)
    {
		rbLifted.useGravity = true;
		colliderLifted.enabled = true;
		// If force == 0 and Lifter was moving via their Rigidbody, we could apply Lifter velocity to objectLifted here.
		rbLifted.AddForce(transform.forward * force);
		objectLifted = null;
    }
	
}
