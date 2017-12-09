using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// 3D MouseLook and Transform
/// </summary>
public class PlayerController : MonoBehaviour 
{
	public float moveSpeed = .15f;

	void Awake () 
	{

	}
	

	void Update () 
	{
		Move();
	}

	/// <summary>
	/// Simple 3D Translation
	/// </summary>
	void Move()
	{
		if (Input.GetAxisRaw("Horizontal") > 0)
		{
			transform.Translate(transform.forward * moveSpeed); // Move Forward
		} else if (Input.GetAxisRaw("Horizontal") < 0)
		{
			transform.Translate(-transform.forward * moveSpeed); // Move Forward// Move Backward
		}

		if (Input.GetAxisRaw("Vertical") > 0)
		{
			transform.Translate(transform.right * moveSpeed);// Strafe Right
		} else if (Input.GetAxisRaw("Vertical") < 0)
		{
			transform.Translate(-transform.right * moveSpeed);// Strafe Left
		}
	}
}
