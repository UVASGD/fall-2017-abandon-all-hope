using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class KeyboardMovement : MonoBehaviour {

    public float speed = 10;
    public float jumpspeed = 100;

    private bool grounded = false;

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
        CheckGrounded();

        Rigidbody2D body = GetComponent<Rigidbody2D>();
		if (Input.GetKey(KeyCode.LeftArrow))
        {
            body.AddForce(Vector2.left * speed);
        }
        if (Input.GetKey(KeyCode.RightArrow))
        {
            body.AddForce(Vector2.right * speed);
        }
        if (grounded && Input.GetKeyDown(KeyCode.UpArrow))
        {
            body.AddForce(Vector2.up * jumpspeed);
        }
	}

    private void CheckGrounded()
    {
        Bounds bounds = GetComponent<Collider2D>().bounds;
        RaycastHit2D hit = Physics2D.BoxCast(bounds.center, bounds.size, 0, Vector2.down, 0.02f);
        grounded = hit.collider != null;
        //if (grounded) print("on ground: " + hit.collider.name);
        //else print("off ground");
    }
}
