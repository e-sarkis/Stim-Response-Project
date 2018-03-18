using System.Collections;
using System.Collections.Generic;
using System; // Included for System.Action
using UnityEngine;


public enum Stimulant
{
	Fire,
	Water
};

/// <summary>
/// Represent catalog of in-game physical stimulants and responses
/// </summary>
public class StimResponseObject : MonoBehaviour 
{
	public List<Stimulant> Stims;				// This Object's Stims represented by Strings
	Dictionary<Stimulant, Action> GlobalStims;	// Dictionary: Stim -> System.Action

	// concurrent bags???
	// Stim as it's own base class
	// Scriptable obj
	// Execute
	// Evetn driven behaviours
	// How do clocks work (second gear)?
	// Subscription based information propagation

	// GameObjects currently collided with
	[HideInInspector] public List<GameObject> gameObjectsTouching;

	void Start () 
	{
		GlobalStims = new Dictionary<Stimulant, Action>();	// Initialize Global Stim Dictionary
		GlobalStims.Add(Stimulant.Fire, FireResponse);			// Fire
		GlobalStims.Add(Stimulant.Water, WaterResponse);		// Water
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
			foreach(Stimulant stim in SRObj.Stims)
			{
				// Execute relevant function
				if (GlobalStims.ContainsKey(stim)) GlobalStims[stim]();
			}
		}
	}

	public void ApplyStim(GameObject other, Stimulant stim)
	{
		StimResponseObject SRObj = other.gameObject.GetComponent<StimResponseObject>();
		if(SRObj != null)	// Does the other gameobject potentially possess stims?
		{
			if (GlobalStims.ContainsKey(stim)) GlobalStims[stim]();
		}
	}

	public IEnumerator addStimDelayed(float delayInSecs, Stimulant stim)
	{
		yield return new WaitForSeconds(delayInSecs);
		Stims.Add(stim);
		// Apply the delayed Stim to our touching adjacent GameObjects
		StimResponseObject srObj;
		foreach (GameObject gObj in gameObjectsTouching)
		{
			srObj = gObj.GetComponent<StimResponseObject>();
			if (srObj != null) srObj.ApplyStim(this.gameObject, stim);
		}
	}

	// Virtual response functions (System.Action) to be overridden
	virtual protected void FireResponse() {	}
	virtual protected void WaterResponse() { }
}
