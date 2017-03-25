using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BulletMovement : MonoBehaviour {

	public Vector2 velocity;
	public double timeout = 2.0;
    public int power = 1;

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

	void OnCollisionEnter2D (Collision2D other)
	{
		if (other.gameObject.tag == "Enemy")
        {
            EnemyConroller enemy = other.gameObject.GetComponent<EnemyConroller>();
            enemy.Hit(power);
        }
		Destroy (this.gameObject);
	}

}
