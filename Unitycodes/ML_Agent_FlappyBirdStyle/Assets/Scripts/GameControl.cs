using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class GameControl : MonoBehaviour {

	// Singleton
	public static GameControl instance;
	public GameObject gameOverText;
	public Text scoreText;
	public bool gameOver = false;
	public float scoreReward = 1f;
	public float scrollSpeed = -3f;

	private int score = 0;

	// Use this for initialization
	void Awake () {
		if (instance == null) {
			instance = this;
		} else if (instance != this) {
			Destroy (gameObject);
		}
	}
	
	// Update is called once per frame
	void FixedUpdate () {
//		if (gameOver == true && Input.GetMouseButtonDown (0)) {
//			SceneManager.LoadScene (SceneManager.GetActiveScene ().buildIndex);
//		}
	}

	public void BirdScored(bool reset){
		// If game over don't add score 
		if (gameOver) {
			return;
		}
		// If bird scored, add score
		if (reset == true) {
			score = 0;
		} else {
			score++;
		}

		scoreText.text = "Score: " + score.ToString ();
	}

//	public void BirdDied()
//	{
//		// Active game over text when bird is died
//		gameOverText.SetActive (true);	
//		gameOver = true;
//	}
}
