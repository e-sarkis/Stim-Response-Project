using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LiftManipulate : MonoBehaviour 
{
	public float initialLiftRange 	= 1.5f;	// Range lifted GameObject is at Lift initiation
	public float throwForce 		= 200f;	// Force applied when Lifter throws lifted GameObject

	public float minLiftDist 		= 0.8f;	// Closest range lifted GameObject can be to Lifter during lifting 
	public float startLiftDist 		= 1;	// Range lifted GameObject is at Lift initiation
	public float maxLiftDist 		= 2.0f;	// Furthest range lifted GameObject can be to Lifter during lifting 
	private float currentLiftDist 	= 0.0f;	// Range of lifted GameObject to Lifter

	[HideInInspector] public GameObject objectLifted;	// GameObject being lifted
	private Rigidbody rbLifted;							// Rigidbody of lifted GameObject
	private Collider colliderLifted;					// Collider of lifted GameObject

	public bool debug = false;
	
	void Update () 
	{
		if (!objectLifted) ObtainLiftable(); // Check for Liftable / Obtain Liftable if possible
		else
		{
			// A GameObject is being lifted
			UpdateLiftedObject();
			if (Input.GetKeyDown(KeyCode.Mouse0)) ThrowLiftedObject();
			if (Input.GetKeyDown(KeyCode.Mouse1)) ThrowLiftedObject(throwForce);
		} 
	}

	// Manipulate lifted GameObject rotation and position
    void UpdateLiftedObject()
	{
		// Distance manipulation
		currentLiftDist += Input.GetAxis("Mouse ScrollWheel");

		//Mathf.Clamp(currentLiftDist, minLiftDist, maxLiftDist);	// Stopped working for unknown reason
		// TEMP - Hardcode replacement for Mathf.Clamp
		if (currentLiftDist < minLiftDist) currentLiftDist = minLiftDist;
		if (currentLiftDist > maxLiftDist) currentLiftDist = maxLiftDist;
		
		// Rotation maniupulation
		if (Input.GetButton("Use"))
		{		
			float mX = Input.GetAxis("Mouse X");
			float mY = Input.GetAxis("Mouse Y");
			objectLifted.transform.Rotate(Vector3.up, -mX, Space.World);
			objectLifted.transform.Rotate(transform.right, mY, Space.World);
		}
		// Update position of lifted GameObject
		objectLifted.transform.position = transform.position + (transform.forward * currentLiftDist);
	}


	// Check for Input, and Grab the GameObject if in range and liftable
	void ObtainLiftable()
	{
		if (Input.GetKeyDown(KeyCode.Mouse0))
		{
			// Lifter attempt to find in-range GameObject
			if (debug) Debug.DrawRay(transform.position, transform.forward, Color.red, initialLiftRange);
			RaycastHit hit;
			if (Physics.Raycast(transform.position, transform.forward, out hit, initialLiftRange))
			{
				objectLifted = hit.transform.gameObject; // Obtained the GameObject
				PrepForManipulation();
			}
		}
	}

	// Adjust physics components of lifted GameObject if confirmed liftable
	void PrepForManipulation()
	{
		PhysicsProperties ppLifted = objectLifted.GetComponent<PhysicsProperties>();
		if (!ppLifted || !ppLifted.liftable)
		{
			objectLifted = null;
			return;	// GameObject was not liftable
		}

		rbLifted = objectLifted.GetComponent<Rigidbody>();
		rbLifted.velocity 			= Vector3.zero;
		rbLifted.angularVelocity 	= Vector3.zero;
		rbLifted.useGravity 		= false;

		colliderLifted = objectLifted.GetComponent<Collider>();
		colliderLifted.enabled = false;

		currentLiftDist = startLiftDist;
		objectLifted.transform.position = transform.position + (transform.forward * currentLiftDist);
		objectLifted.transform.parent = this.transform;	// Preserves relativity of rotation when Lifter looks around
	}

	// Drop the currently lifted GameObject, and apply given force if provided
	void ThrowLiftedObject(float force = 0f)
    {
		rbLifted.useGravity = true;
		colliderLifted.enabled = true;
		// If force == 0 and Lifter was moving via their Rigidbody, we could apply Lifter velocity to objectLifted here.
		rbLifted.AddForce(transform.forward * force);
		objectLifted.transform.parent = null;
		objectLifted = null;
    }
	
}
