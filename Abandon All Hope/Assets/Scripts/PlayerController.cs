using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class PlayerController : MonoBehaviour {
    const int LEFT = -1, RIGHT = 1;

    public float speed = 40;
    public float jumpspeed = 1000;
    public int maxhealth = 10;
    public int maxlives = 5;

    //public Text healthBar;
    //public Text numLives;

	public BulletMovement bullet;
	public float bulletSpeed = 0.5f;
    private bool grounded = false;
	private int facing = 1;
    private int health;
    private int lives;

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
        if (!CheckGrounded())
        {
            if (body.velocity.y < 0)
            {
                //print("falling");
            } else {
                //print("rising");
            }
        }
        SpriteRenderer sprite = GetComponent<SpriteRenderer>();
        sprite.flipX = facing == LEFT;
        if(body.position.y < -25)
        {
            Die();
        }
    }

    private bool CheckGrounded()
    {
        int hits = GetComponent<Rigidbody2D>().Cast(Vector2.down, new RaycastHit2D[1], 0.02f);
        return grounded = hits > 0;
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
        //death animation
        SceneManager.LoadScene("death scene level 1");
        print(name + " died!!1");
    }

}
