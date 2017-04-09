using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour {
    const int LEFT = -1, RIGHT = 1;

    public float speed = 40;
    public float jumpspeed = 1000;
    public int maxhealth = 10;

	public BulletMovement bullet;
	public float bulletSpeed = 0.5f;

	private int facing = 1;
    private int health;

	// Use this for initialization
	void Start () {
        health = maxhealth;
	}
	
	// Update is called once per frame
	void Update () {
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
        if (Input.GetKeyDown(KeyCode.UpArrow) && CheckGrounded())
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

    private bool CheckGrounded()
    {
        return GetComponent<Rigidbody2D>().Cast(Vector2.down, new RaycastHit2D[1], 0.02f) > 0;
        //Bounds bounds = GetComponent<Collider2D>().bounds;
        //RaycastHit2D hit = Physics2D.BoxCast(bounds.center, bounds.size, 0, Vector2.down, 0.02f);
        //return grounded = hit.collider != null;
    }

    public void Hit(int damage)
    {
        //print(name + " hit: " + damage + " damage");
        health -= damage;
        if (health <= 0)
            Die();
    }

    private void Die()
    {
        print(name + " died!!1");
        
    }

}
