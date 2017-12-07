using System.Collections;
using System.Collections.Generic;
using System; // Included for System.Action
using UnityEngine;

/// <summary>
/// Represent catalog of in-game physical stimulants and responses
/// </summary>
public class StimResponseObject : MonoBehaviour 
{
	public List<string> Stims;				// All ingame Stims represented by Strings
	Dictionary<string, Action> GlobalStims;	// Dictionary Stim string -> System.Action

	void Awake () 
	{
		GlobalStims = new Dictionary<string, Action>();	// Initialize Global Stim Dictionary
		GlobalStims.Add("Fire", FireResponse);			// Fire
		GlobalStims.Add("Water", WaterResponse);		// Water
	}

	void OnCollisionEnter(Collision other)
	{
		StimResponseObject SRObj = other.gameObject.GetComponent<StimResponseObject>();
		if(SRObj != null)	// Does the other gameobject potentially possess stims?
		{
			foreach(string stim in SRObj.Stims)
			{
				// Execute relevant function
				if (GlobalStims.ContainsKey(stim)) GlobalStims[stim]();
			}
		}
	}

	// Virtual response functions (System.Action) to be overridden
	virtual protected void FireResponse() {	}
	virtual protected void WaterResponse() { }
}
