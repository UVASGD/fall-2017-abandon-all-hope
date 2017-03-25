using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GlobalInputListener : MonoBehaviour {

	// Use this for initialization
	void Awake () {
		DontDestroyOnLoad (transform.gameObject);
	}
	
	// Update is called once per frame
	void Update () {
		if (Input.GetKeyDown ("escape")) {
			Application.Quit ();
		}
	}
}
