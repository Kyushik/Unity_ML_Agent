using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Column : MonoBehaviour {

	void FixedUpdate(){
		// Define GameObject list which has columnPoolSize 
		GameObject Agent = GameObject.FindGameObjectWithTag ("Bird");
		BirdAgent BirdAgentScript = Agent.GetComponent<BirdAgent> ();

		BirdAgentScript.SendMessage ("RewardScore", 0f);
	}

	// If something collide on Trigger
	private void OnTriggerEnter2D (Collider2D other) {
		// if collided object is bird
		if (other.GetComponent<BirdAgent> () != null) {
			BirdAgent BirdAgentScript = other.GetComponent<BirdAgent> ();

			BirdAgentScript.SendMessage ("RewardScore", 1.0f);

			GameControl.instance.BirdScored(false);
		}
	}
}
