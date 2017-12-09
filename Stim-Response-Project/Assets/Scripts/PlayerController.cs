using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// 3D MouseLook and Transform
/// </summary>
public class PlayerController : MonoBehaviour 
{
	float moveForce = 1f;

	void Start () 
	{
		
	}
	

	void Update () 
	{
		Move();
	}

	void Move()
	{
		if (Input.GetAxisRaw("Horizontal") > 0)
		{
			// Move Forward
		} else if (Input.GetAxisRaw("Horizontal") < 0)
		{
			// Move Backward
		}

		if (Input.GetAxisRaw("Vertical") > 0)
		{
			// Strafe Right
		} else if (Input.GetAxisRaw("Vertical") < 0)
		{
			// Strafe Left
		}
	}
}
