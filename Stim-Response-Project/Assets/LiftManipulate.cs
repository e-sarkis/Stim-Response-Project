using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LiftManipulate : MonoBehaviour 
{
	float initialLiftRange = 1f;

	GameObject objectLifted;

	// Use this for initialization
	void Start () 
	{
		
	}
	
	// Update is called once per frame
	void Update () 
	{
		if (!objectLifted) ObtainLiftable(); 		// Check for Liftable / Obtain Liftable
		else UpdateLiftedObject();	// Perform Updates on Current Liftable
	}

	void UpdateLiftedObject()
	{
		objectLifted.transform.position = transform.position + (transform.forward * initialLiftRange);
	}


	// Check for Input, and Grab the GameObject if in range and liftable
	void ObtainLiftable()
	{
		if (Input.GetKey(KeyCode.Mouse0))
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
		Rigidbody rb = objectLifted.GetComponent<Rigidbody>();
		rb.useGravity = false;
		Collider coll = objectLifted.GetComponent<Collider>();
		coll.enabled = false;
	}
	
}
