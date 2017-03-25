using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour {
    const int LEFT = -1, RIGHT = 1;

    public float speed = 40;
    public float jumpspeed = 1000;

	public BulletMovement bullet;
	public float bulletSpeed = 0.5f;

    private bool grounded = false;
	private int facing = 1;

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
			facing = LEFT;
        }
        if (Input.GetKey(KeyCode.RightArrow))
        {
            body.AddForce(Vector2.right * speed);
			facing = RIGHT;
        }
        if (grounded && Input.GetKeyDown(KeyCode.UpArrow))
        {
            body.AddForce(Vector2.up * jumpspeed);
        }
		if (Input.GetKeyDown(KeyCode.Space))
		{
			BulletMovement bullet2 = Instantiate(bullet, transform.position + new Vector3(.7f * facing, .1f), Quaternion.identity);
			bullet2.velocity = new Vector2(bulletSpeed * facing, 0);
		}
        SpriteRenderer sprite = GetComponent<SpriteRenderer>();
        sprite.flipX = facing == LEFT;
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
