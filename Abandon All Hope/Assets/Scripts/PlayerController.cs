using System.Collections;
using System.Collections.Generic;
using UnityEngine.SceneManagement;
using UnityEngine;

public class PlayerController : MonoBehaviour {
    const int LEFT = -1, RIGHT = 1;

    int prev_dir = 1;

    public float speed = 40;
    public float jumpspeed = 1000;
    public int maxhealth = 10;
	//public Vector2 jumpHeight;

	public BulletController bullet;
	public float bulletSpeed = 4;

   // private bool grounded = false;
	private int facing = 1;
    public int health;

    private float old_pos;

    private int sprintFrames = 0;

	// Use this for initialization
	void Start () {
        health = maxhealth;
        old_pos = transform.position.x;
	}

	// Update is called once per frame
	void Update () {
		AudioSource shoot = GetComponent<AudioSource>();
        Rigidbody2D body = GetComponent<Rigidbody2D>();
        if(old_pos == transform.position.x && prev_dir ==facing)
        {
            sprintFrames = 0;
            print("still");
        }
		if (Input.GetKey(KeyCode.LeftArrow))
        {
            if(CheckGrounded())
            {
                sprintFrames++;
            }

            if (sprintFrames >= 90)
            {
                body.AddForce(Vector2.left * (speed+15));
                print("Sprinting");
            }
            else
            {
                body.AddForce(Vector2.left * speed);
            }

			facing = LEFT;

        }
        if (Input.GetKey(KeyCode.RightArrow))
        {
            if (CheckGrounded())
            {
                sprintFrames++;
            }
            if (sprintFrames >= 90)
            {
                body.AddForce(Vector2.right * (speed + 15));
                print("Sprinting");
            }
            else
            {
                body.AddForce(Vector2.right * speed);
            }
            facing = RIGHT;
        }
        if (Input.GetKeyDown(KeyCode.UpArrow) && CheckGrounded())
        {
			//updated jump - maybe less floaty now? -Susannah
			GetComponent<Rigidbody2D>().AddForce(new Vector2(0,20), ForceMode2D.Impulse);
           // body.AddForce(Vector2.up * jumpspeed);
           // sprintFrames = 0;
        }
        if (Input.GetKey(KeyCode.UpArrow)){
            if(GetComponent<Rigidbody2D>().velocity.y > 6)
                GetComponent<Rigidbody2D>().AddForce(new Vector2(0.0f, 1.3f), ForceMode2D.Impulse);
            else GetComponent<Rigidbody2D>().AddForce(new Vector2(0.0f, 0.5f), ForceMode2D.Impulse); // Hover mechanic

        }
		if (Input.GetKeyDown(KeyCode.Space)) {
			shoot.Play ();
            Shoot();
		}
        SpriteRenderer sprite = GetComponent<SpriteRenderer>();
        sprite.flipX = facing == LEFT;
        if (!CheckGrounded() && body.position.y < -25)
        {
            Die();
        }

        old_pos = transform.position.x;
    }

    public int Health {
        get { return health; }
    }

	public void setHealth(int health) {
		this.health = health;
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
        SceneManager.LoadScene("death screen");
    }

    private void Shoot()
    {
        Vector2 position = transform.position + new Vector3(.6f * facing, 0f);
        BulletController bullet2 = Instantiate(bullet, position, Quaternion.identity);
        bullet2.Initialize(new Vector2(bulletSpeed * facing, 0), false);
    }
}

public enum DashState
{
    Ready,
    Dashing,
    Cooldown
}
