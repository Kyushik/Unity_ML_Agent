using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ScrollingObject : MonoBehaviour {

	private Rigidbody2D rb2d;

	// Use this for initialization
	void Start () {
		// Define Rigidbody2D
		rb2d = GetComponent<Rigidbody2D> ();
		// Set velocity of the ground
		rb2d.velocity = new Vector2 (GameControl.instance.scrollSpeed, 0);
	}
	
	// Update is called once per frame
	void FixedUpdate () {
		// If game is over, ground stop scrolling
		if (GameControl.instance.gameOver == true) {
			rb2d.velocity = Vector2.zero;
		}
	}
}
