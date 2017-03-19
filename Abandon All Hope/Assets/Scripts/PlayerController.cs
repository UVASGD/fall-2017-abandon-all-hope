using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour {

    public float speed = 10;
    public float jumpspeed = 100;

	public BulletMovement bullet;
	public float bulletSpeed = 0.5f;

    private bool grounded = false;
	private int facingRight = 1;

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
			facingRight = -1;
        }
        if (Input.GetKey(KeyCode.RightArrow))
        {
            body.AddForce(Vector2.right * speed);
			facingRight = 1;
        }
        if (grounded && Input.GetKeyDown(KeyCode.UpArrow))
        {
            body.AddForce(Vector2.up * jumpspeed);
        }
		if (Input.GetKeyDown(KeyCode.Space))
		{
			BulletMovement bullet2 = Instantiate(bullet, transform.position + new Vector3(.7f * facingRight, .1f), Quaternion.identity);
			bullet2.velocity = new Vector2(bulletSpeed * facingRight, 0);
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
