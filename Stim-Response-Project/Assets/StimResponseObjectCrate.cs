﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StimResponseObjectCrate : StimResponseObject 
{
	[SerializeField] private Color albedoOnFire;
	private Color initialAlbedo;

	float timeUntilFireContagion = 2.0f;

	void Awake()
	{
		initialAlbedo = GetComponent<MeshRenderer>().material.color;
	}

	override protected void FireResponse() 
	{
		if (Stims.Contains("Fire")) return;	// Already on fire
		GetComponent<MeshRenderer>().material.color = albedoOnFire;
		StartCoroutine(addStimDelayed(timeUntilFireContagion, "Fire"));
	}

	override protected void WaterResponse() 
	{
		GetComponent<MeshRenderer>().material.color = initialAlbedo;
		Stims.Remove("Fire");
		StopCoroutine("addStimDelayed");	// Temporary - Stops delayed Fire stim but also all other delays
	}
}
