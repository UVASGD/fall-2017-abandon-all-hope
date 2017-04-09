using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BulletController : MonoBehaviour {

	public Vector2 velocity;
	public double timeout = 2.0;
    public int power = 1;
    public bool isBad = false;

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		GetComponent<Rigidbody2D>().velocity = velocity;
		timeout -= Time.deltaTime;
		if (timeout <= 0) {
			Destroy(this.gameObject);
		}
	}

	void OnCollisionEnter2D (Collision2D other)
	{
        if (isBad && other.gameObject.CompareTag("Player"))
        {
            PlayerController player = other.gameObject.GetComponent<PlayerController>();
            player.Hit(power);
        }
        else if (!isBad && other.gameObject.CompareTag("Enemy"))
        {
            EnemyConroller enemy = other.gameObject.GetComponent<EnemyConroller>();
            enemy.Hit(power);
        }
		Destroy (this.gameObject);
	}

}
