using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class DistanceText : MonoBehaviour {

    public static float dist;
    Text text;

	// Use this for initialization
	void Start () {
        text = GetComponent<Text>();
        dist = 0;
	}
	
	// Update is called once per frame
	void Update () {
        text.text = "Distance: " + dist.ToString("F2"); 
	}
}
