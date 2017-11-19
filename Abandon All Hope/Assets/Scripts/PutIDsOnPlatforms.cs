using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PutIDsOnPlatforms : MonoBehaviour {

	// Use this for initialization
	void Start () {
		int i = 0;
		foreach (Transform child in transform) {
			child.gameObject.GetComponent<PlatformID> ().platformID = i;
			i++;
		}
	}
}
