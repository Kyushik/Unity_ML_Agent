using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RepeatingBackground : MonoBehaviour {

	private BoxCollider2D groundCollider;
	private float groundHorizontalLength;

	// Use this for initialization
	void Start () {
		// Get collider component and x size
		groundCollider = GetComponent<BoxCollider2D> ();
		groundHorizontalLength = groundCollider.size.x;
	}
	
	// Update is called once per frame
	void FixedUpdate () {
		// If ground is out of the screen
		if (transform.position.x < -groundHorizontalLength) {
			// Reposition background
			RepositionBackground ();
		}
	}

	private void RepositionBackground(){
		Vector2 groundOffset = new Vector2 (groundHorizontalLength * 2f, 0);
		transform.position = (Vector2)transform.position + groundOffset;
	}
}
