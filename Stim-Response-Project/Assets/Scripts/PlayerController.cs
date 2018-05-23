using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// 3D MouseLook and Transform
/// </summary>
public class PlayerController : MonoBehaviour 
{
	[Header("Movement Settings")]
	[Range(1.0f, 15.0f)]
	[Tooltip("Movement speed in Unity units per second")]
	public float moveSpeed = .05f;

	[Header("Mouselook Settings")]
	[Range(0.0f, 100.0f)]
	public float mouseSensitivity 	= 50f;
	[Range(60.0f, 90.0f)]
	[Tooltip("Maximum vertical looking angle magnitude (X axis rotation)")]
	public float angleClamp 		= 75f;

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
		if (!Input.GetButton("Use"))
		{
			MouseLook();
		}

		Move();
		// Simple Clamping to Floor
		transform.position = new Vector3(transform.position.x, 1f, transform.position.z);
	}

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
		// Move Forward or Backward
		transform.Translate(Input.GetAxisRaw("Vertical")	* Vector3.forward
							* moveSpeed * Time.deltaTime);
		// Strafe Right
		transform.Translate(Input.GetAxisRaw("Horizontal")	* Vector3.right
							* moveSpeed* Time.deltaTime);
	}
}
