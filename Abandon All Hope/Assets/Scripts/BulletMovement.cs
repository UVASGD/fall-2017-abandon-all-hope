using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BulletMovement : MonoBehaviour {

	public Vector2 velocity;
	private double timeout = 5.0;
	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		transform.position += (Vector3) velocity;
		timeout -= Time.deltaTime;
		if (timeout <= 0) {
			Destroy(this.gameObject);
		}
	}

	void OnTriggerEnter2D (Collider2D other)
	{
		// TODO: Check tag
		Destroy (this.gameObject);
	}

	void OnCollisionEnter2D (Collision2D other)
	{
		// TODO: Check tag
		Destroy (this.gameObject);
		print("hi");
	}

    
}
