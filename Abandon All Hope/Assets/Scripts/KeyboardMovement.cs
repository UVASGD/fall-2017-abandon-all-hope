using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class KeyboardMovement : MonoBehaviour {

    public float speed = 10;
    public float jumpspeed = 100;

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
        Rigidbody2D body = GetComponent<Rigidbody2D>();
		if (Input.GetKey(KeyCode.LeftArrow))
        {
            body.AddForce(Vector2.left * speed);
        }
        if (Input.GetKey(KeyCode.RightArrow))
        {
            body.AddForce(Vector2.right * speed);
        }
        if (Input.GetKeyDown(KeyCode.UpArrow))
        {
            body.AddForce(Vector2.up * jumpspeed);
        }
	}
}
