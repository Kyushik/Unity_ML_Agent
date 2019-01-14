using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class followAgent : MonoBehaviour {
    public GameObject foot; 

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
        this.transform.position = new Vector3(foot.transform.position.x, this.transform.position.y, this.transform.position.z);
	}
}
