using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StimResponseObjectCrate : StimResponseObject 
{
	[SerializeField] private Color albedoOnFire;
	private Color initialAlbedo;

	void Awake()
	{
		initialAlbedo = GetComponent<MeshRenderer>().material.color;
	}

	override protected void FireResponse() 
	{
		GetComponent<MeshRenderer>().material.color = albedoOnFire;
	}

	override protected void WaterResponse() 
	{
		GetComponent<MeshRenderer>().material.color = initialAlbedo;
	}
}
