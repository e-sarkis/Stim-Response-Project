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

	[Header("Headbob Settings")]
	[Tooltip("Headbob magnitude in Unity units per bob")]
	[Range(0.0f, 0.2f)]
	public float headbobMagnitude	= 0.05f;
	[Tooltip("Smoothing interpolation value of headbob transition from moving to not moving per second.")]
	[Range(10.0f, 20.0f)]
	public float headbobTransitionLerpValue = 20f;
	[Tooltip("Headbob speed in Unity units per second")]
	public float headbobSpeed 		= 4.8f;

	private float _rotX = 0f;
	private float _rotY = 0f;

	private Vector3 _headbobRestPosition; // Local transform position of player Camera at rest
	private float _headbobTimer = Mathf.PI / 2; // Sin(PI / 2) = 1;

	void Awake () 
	{
		Vector3 rot = transform.localRotation.eulerAngles;
		_rotX = rot.x;
		_rotY = rot.y;

		_headbobRestPosition = GetComponentInChildren<Camera>().transform.position;
	}
	

	void Update () 
	{
		// No MouseLook when attempting object manipulation via "Use" button
		if (!Input.GetButton("Use"))
		{
			MouseLook();
		}

		Move();
		Headbob();
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

		_rotY += mX * mouseSensitivity * Time.deltaTime;
		_rotX += -mY * mouseSensitivity * Time.deltaTime;
		_rotX = Mathf.Clamp(_rotX, -angleClamp, angleClamp);

		Quaternion localRot = Quaternion.Euler(_rotX, _rotY, 0f);
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

	/// <summary>
	/// Simple 3D headbobbing
	/// </summary>
	void Headbob()
	{
		Transform cameraTransform = GetComponentInChildren<Camera>().transform;
		float nextY; // The target bob position on Y axis for this Update
		
		// Determine target bob position on Y axis for this Update
		if (Input.GetAxisRaw("Horizontal") != 0 || Input.GetAxisRaw("Vertical") != 0)
		{
			_headbobTimer += headbobSpeed * Time.deltaTime;
			nextY = Mathf.Lerp(	cameraTransform.position.y,
								_headbobRestPosition.y + Mathf.Abs((Mathf.Sin(_headbobTimer) * headbobMagnitude)),
								headbobTransitionLerpValue * Time.deltaTime);
		} else
		{
			_headbobTimer = Mathf.PI / 2;
			nextY = Mathf.Lerp(	cameraTransform.position.y,
								_headbobRestPosition.y,
								headbobTransitionLerpValue * Time.deltaTime);
		}

		// Set the new camera position
		Vector3 nextPosition = new Vector3(	cameraTransform.position.x, nextY, cameraTransform.position.z);
		cameraTransform.position = nextPosition;

		// Simple _headbobTimer clamp to keep trig values meaningful
		if (_headbobTimer > Mathf.PI * 2)
		{
			_headbobTimer -= Mathf.PI * 2;
		}
	}
}
