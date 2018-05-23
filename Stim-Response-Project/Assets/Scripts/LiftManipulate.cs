using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LiftManipulate : MonoBehaviour 
{
	[Header("Distance Lifting Settings")]
	[Tooltip("Liftable object detection range on Lift initiation")]
	public float initialLiftRange 	= 1.5f;
	[Tooltip("Force applied when Lifter throws lifted GameObject")]
	public float throwForce 		= 200f;

	[Tooltip("Closest range lifted GameObject can be to Lifter during lifting")]
	public float minLiftDist 		= 0.8f;
	[Tooltip("Range lifted GameObject is at Lift initiation")]
	public float startLiftDist 		= 1;
	[Tooltip("Furthest range lifted GameObject can be to Lifter during lifting")]
	public float maxLiftDist 		= 2.0f;	// Furthest range lifted GameObject can be to Lifter during lifting 
	[Tooltip("Smoothing interpolation value of headbob transition from moving to not moving per second.")]
	public float liftDistLerp 		= 1.0f; 
	private float _currentLiftDist 	= 0.0f;	// Range of lifted GameObject to Lifter

	[HideInInspector] public GameObject objectLifted;	// GameObject being lifted
	private Rigidbody _rbLifted;							// Rigidbody of lifted GameObject
	private Collider _colliderLifted;					// Collider of lifted GameObject

	[Header("DEBUG")]
	public bool debug = false;
	
	void Update () 
	{
		if (!objectLifted)
		{
			ObtainLiftable(); // Check for Liftable / Obtain Liftable if possible
		} else
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
		_currentLiftDist += Input.GetAxis("Mouse ScrollWheel");

		//Mathf.Clamp(currentLiftDist, minLiftDist, maxLiftDist);	// Stopped working for unknown reason
		// TEMP - Hardcode replacement for Mathf.Clamp
		if (_currentLiftDist < minLiftDist) _currentLiftDist = minLiftDist;
		if (_currentLiftDist > maxLiftDist) _currentLiftDist = maxLiftDist;
		
		// Rotation maniupulation
		if (Input.GetButton("Use"))
		{		
			float mX = Input.GetAxis("Mouse X");
			float mY = Input.GetAxis("Mouse Y");
			objectLifted.transform.Rotate(Vector3.up, -mX, Space.World);
			objectLifted.transform.Rotate(transform.right, mY, Space.World);
		}
		// Update position of lifted GameObject
		objectLifted.transform.position = Vector3.Lerp(
										objectLifted.transform.position,
										transform.position + (transform.forward * _currentLiftDist),
										liftDistLerp * Time.deltaTime);
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

		_rbLifted = objectLifted.GetComponent<Rigidbody>();
		_rbLifted.velocity 			= Vector3.zero;
		_rbLifted.angularVelocity 	= Vector3.zero;
		_rbLifted.useGravity 		= false;

		_colliderLifted = objectLifted.GetComponent<Collider>();
		_colliderLifted.enabled = false;

		_currentLiftDist = startLiftDist;
		
		objectLifted.transform.position = transform.position + (transform.forward * _currentLiftDist);
		objectLifted.transform.parent = this.transform;	// Preserves relativity of rotation when Lifter looks around
	}

	// Drop the currently lifted GameObject, and apply given force if provided
	void ThrowLiftedObject(float force = 0f)
    {
		_rbLifted.useGravity = true;
		_colliderLifted.enabled = true;
		// If force == 0 and Lifter was moving via their Rigidbody, we could apply Lifter velocity to objectLifted here.
		_rbLifted.AddForce(transform.forward * force);
		objectLifted.transform.parent = null;
		objectLifted = null;
    }
	
}
