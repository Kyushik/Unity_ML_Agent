using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ColumnPool : MonoBehaviour {
	// 5 columns
	public int columnPoolSize = 5;
	public GameObject columnPrefab;
	public float spawnRate = 2f; // How often are we gonna spawn column
	public float columnMin = -1.5f;
	public float columnMax = 3.5f;

	private GameObject[] columns;
	private Vector2 objectPoolPosition = new Vector2 (-15f, -25f);
	private float timeSinceLasetSpawned; // Add timer
	private float spawnXPosition = 5f; // X position offset of column
	private int currentColumn = 0;
	private bool init_game = true;

	// Use this for initialization
	void Start () {
		// Define GameObject list which has columnPoolSize 
		columns = new GameObject[columnPoolSize];
		for (int i = 0;	i < columnPoolSize; i++) {
			// Instantiate Gameobject (Prefab, position, rotation)
			columns [i] = (GameObject)Instantiate (columnPrefab, objectPoolPosition, Quaternion.identity);
		}
	}
	
	// Update is called once per frame
	void FixedUpdate () {
		GameObject Agent = GameObject.FindGameObjectWithTag ("Bird");
		BirdAgent BirdAgentScript = Agent.GetComponent<BirdAgent> ();
		bool is_dead = BirdAgentScript.isDead;

		init_game = false;

		if (is_dead == true) {
			for (int i = 0;	i < columnPoolSize; i++) {
				columns[i].transform.position = objectPoolPosition;
				init_game = true;
			}
		}

		timeSinceLasetSpawned += 0.015f;

		// If more than spawnRate after last spawn, spawn new column
		if ((GameControl.instance.gameOver == false && timeSinceLasetSpawned >= spawnRate) || (init_game == true)) {
			timeSinceLasetSpawned = 0;
			// Make random y position
			float spawnYPosition = Random.Range (columnMin, columnMax);

			// Position column
			columns[currentColumn].transform.position = new Vector2(spawnXPosition, spawnYPosition);

			// Add column index 
			currentColumn++;

			// Initialize the column index 
			if (currentColumn >= columnPoolSize) {
				currentColumn = 0;
			}
		}
	}
}
