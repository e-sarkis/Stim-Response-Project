using System.Collections;
using System.Collections.Generic;
using System; // Included for System.Action
using UnityEngine;

/// <summary>
/// Represent catalog of in-game physical stimulants and responses
/// </summary>
public class StimResponseObject : MonoBehaviour 
{
	public List<string> Stims;				// This Object's Stims represented by Strings
	Dictionary<string, Action> GlobalStims;	// Dictionary Stim string -> System.Action

	// GameObjects currently collided with
	[HideInInspector] public List<GameObject> gameObjectsTouching;

	void Start () 
	{
		GlobalStims = new Dictionary<string, Action>();	// Initialize Global Stim Dictionary
		GlobalStims.Add("Fire", FireResponse);			// Fire
		GlobalStims.Add("Water", WaterResponse);		// Water
	}

	void OnCollisionEnter(Collision other)
	{
		gameObjectsTouching.Add(other.gameObject);
		ApplyStims(other.gameObject);
	}

	void OnCollisionExit(Collision other)
	{
		gameObjectsTouching.Remove(other.gameObject);
	}

	public void ApplyStims(GameObject other)
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

	public IEnumerator addStimDelayed(float delayInSecs, string stim)
	{
		yield return new WaitForSeconds(delayInSecs);
		Stims.Add(stim);
		// Apply the delayed Stim to our touching adjacent GameObjects
		StimResponseObject srObj;
		foreach (GameObject gObj in gameObjectsTouching)
		{
			srObj = gObj.GetComponent<StimResponseObject>();
			if (srObj != null) srObj.ApplyStims(this.gameObject);
		}
	}

	// Virtual response functions (System.Action) to be overridden
	virtual protected void FireResponse() {	}
	virtual protected void WaterResponse() { }
}
