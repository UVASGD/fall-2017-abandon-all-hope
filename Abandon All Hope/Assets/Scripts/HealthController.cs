using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HealthController : MonoBehaviour {
	//private bool TouchingPlayer = false;
	// Use this for initialization
	public AudioClip healthSound;
	void OnTriggerEnter2D(Collider2D other)
	{
		AudioSource source = GetComponent<AudioSource>();
		SpriteRenderer renderer = GetComponent<SpriteRenderer> ();
		if (other.gameObject.name == "Player")
		{
			//TouchingPlayer = false;
			PlayerController player = other.gameObject.GetComponent<PlayerController>();
			if (player.Health != player.maxhealth) {
				player.setHealth (player.Health + 2);
				source.PlayOneShot (healthSound);
				renderer.enabled = false;
				Destroy (gameObject, healthSound.length);
			}

		}
	}
}
