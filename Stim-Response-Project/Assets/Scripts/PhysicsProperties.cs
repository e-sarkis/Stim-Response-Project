using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Checkable Physics Properties
/// </summary>
public class PhysicsProperties : MonoBehaviour 
{
	public bool liftable = false;	// True - GameObject can be Lifted by Lifters

	/// <summary>
	/// OnCollisionEnter is called when this collider/rigidbody has begun
	/// touching another rigidbody/collider.
	/// </summary>
	/// <param name="other">The Collision data associated with this collision.</param>
	void OnCollisionEnter(Collision other)
	{
		if (liftable)
		{
			// TODO - Release
		}
	}
}
