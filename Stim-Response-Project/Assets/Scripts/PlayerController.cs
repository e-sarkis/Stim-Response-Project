using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// 3D MouseLook and Transform
/// </summary>
public class PlayerController : MonoBehaviour 
{
	public float moveSpeed = .15f;

	private float rotX = 0f;
	private float rotY = 0f;

	void Awake () 
	{
		Vector3 rot = transform.localRotation.eulerAngles;
		rotX = rot.x;
		rotY = rot.y;
	}
	

	void Update () 
	{
		// No MouseLook when attempting object manipulation via "Use" button
		if (!Input.GetButton("Use")) MouseLook();

		Move();
		// Simple Clamping to Floor
		transform.position = new Vector3(transform.position.x, 1f, transform.position.z);
	}


	public float mouseSensitivity 	= 50f;
	public float angleClamp 		= 75f;

	/// <summary>
	/// Simple 3D MouseLook
	/// </summary>
	void MouseLook()
	{
		float mX = Input.GetAxis("Mouse X");
		float mY = Input.GetAxis("Mouse Y");

		rotY += mX * mouseSensitivity * Time.deltaTime;
		rotX += -mY * mouseSensitivity * Time.deltaTime;
		rotX = Mathf.Clamp(rotX, -angleClamp, angleClamp);

		Quaternion localRot = Quaternion.Euler(rotX, rotY, 0f);
		transform.rotation = localRot;
	}

	/// <summary>
	/// Simple 3D Translation
	/// </summary>
	void Move()
	{
		if (Input.GetAxisRaw("Vertical") > 0)
		{
			transform.Translate(Vector3.forward * moveSpeed); // Move Forward
		} else if (Input.GetAxisRaw("Vertical") < 0)
		{
			transform.Translate(-Vector3.forward * moveSpeed); // Move Forward// Move Backward
		}

		if (Input.GetAxisRaw("Horizontal") > 0)
		{
			transform.Translate(Vector3.right * moveSpeed);// Strafe Right
		} else if (Input.GetAxisRaw("Horizontal") < 0)
		{
			transform.Translate(-Vector3.right * moveSpeed);// Strafe Left
		}
	}
}
