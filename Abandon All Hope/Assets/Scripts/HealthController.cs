using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HealthController : MonoBehaviour {
	//private bool TouchingPlayer = false;
	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
		
		
	}


	void OnTriggerEnter2D(Collider2D other)
	{
		AudioSource health = GetComponent<AudioSource>();
		if (other.gameObject.name == "Player")
		{
			//TouchingPlayer = false;
			health.Play (); //current health sound starts to play but is destroyed before it can complete
			PlayerController player = other.gameObject.GetComponent<PlayerController>();
			player.setHealth (player.Health + 2);
			Destroy (this.gameObject);
		}
	}
}
