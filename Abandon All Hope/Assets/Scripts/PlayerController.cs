using System.Collections;
using System.Collections.Generic;
using UnityEngine.SceneManagement;
using UnityEngine;

public class PlayerController : MonoBehaviour {
    const int LEFT = -1, RIGHT = 1;

    int prev_dir = 1;

	public float speed;
	public float jumpspeed;
	public float sprintspeed;
	public float midairaccel;
	public float jumpVelDampen;
    public int maxhealth = 10;
    public bool invincible = false;
	//public Vector2 jumpHeight;

	public BulletController bullet;
	public float bulletSpeed = 15;

   // private bool grounded = false;
	private int facing = 1;
    public int health;

    private int sprintFrames = 0;
	private int framesUntilSprint = 70;
	private int jumpTimer = 0;
	private int jumpCooldown = 3; //number of frames before you can jump again. Used to prevent multiple jumps triggering in the space of one jump.

    private Animator anim;
    private Transform shoot_loc;
    private float shoot_offx;
	private AudioSource shoot;
	public AudioClip shotSound;
	private Rigidbody2D body;
    private SpriteRenderer sprite;
    
    // Use this for initialization
    void Start () {
        health = maxhealth;
		shoot = GetComponent<AudioSource>();
		body = GetComponent<Rigidbody2D>();
        sprite = GetComponent<SpriteRenderer>();
        anim = GetComponent<Animator>();
        shoot_loc = transform.FindChild("Shoot_Loc");
        shoot_offx = shoot_loc.localPosition.x;
	}
	
	// Update is called once per frame
	void Update () {
        if (CheckGrounded()) {
            if (Input.GetKey(KeyCode.LeftArrow) && Input.GetKey(KeyCode.RightArrow)) {
                sprintFrames = 0;
                body.velocity = new Vector2(0, body.velocity.y);
            } else if (Input.GetKey(KeyCode.LeftArrow)) {
                sprintFrames++;
                facing = LEFT;
                if (sprintFrames >= framesUntilSprint) {
                    body.velocity = new Vector2(-sprintspeed, 0);
                } else {
                    body.velocity = new Vector2(-speed, 0);
                }
            } else if (Input.GetKey(KeyCode.RightArrow)) {
                sprintFrames++;
                facing = RIGHT;
                if (sprintFrames >= framesUntilSprint) {
                    body.velocity = new Vector2(sprintspeed, 0);
                } else {
                    body.velocity = new Vector2(speed, 0);
                }
            } else {
                sprintFrames = 0;
                body.velocity = new Vector2(0, body.velocity.y);
            }
            if (Input.GetKey(KeyCode.UpArrow) && jumpTimer == 0) {
                sprintFrames = 0;
                jumpTimer = jumpCooldown;
                body.AddForce(new Vector2(0, jumpspeed), ForceMode2D.Impulse);
                body.AddForce(new Vector2(body.velocity.x * jumpVelDampen, 0));
            }
        } else {
            if (Input.GetKey(KeyCode.LeftArrow) && Input.GetKey(KeyCode.RightArrow)) {
            } else if (Input.GetKey(KeyCode.LeftArrow)) {
                facing = LEFT;
                body.AddForce(new Vector2(-midairaccel, 0));
            } else if (Input.GetKey(KeyCode.RightArrow)) {
                facing = RIGHT;
                body.AddForce(new Vector2(midairaccel, 0));
            } else {
            }
        }

        if (jumpTimer > 0) {
            jumpTimer--;
        }
        if (!CheckGrounded() && body.position.y < -25) {
            Die();
        }
        
        prev_dir = facing;
        if (Input.GetKeyDown(KeyCode.Space)) 
            Shoot();

        // update animation
        anim.SetBool("OnGround", CheckGrounded());
        anim.SetBool("Moving", Mathf.Abs(body.velocity.x) > 0);
        anim.SetBool("Dashing", (sprintFrames >= framesUntilSprint));
        sprite.flipX = (facing == LEFT);
        shoot_loc.localPosition = new Vector3(facing * shoot_offx, 0, 0);
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
        if (invincible) return;
        //print(name + " hit: " + damage + " damage");
        health -= damage;
        if (health <= 0)
            Die();
    }

    private void Die()
    {
        //print(name + " died!!1");
        SceneManager.LoadScene("death screen");
    }

    private void Shoot()
    {
        anim.SetTrigger("Shoot");
        Vector2 position = shoot_loc.position;
        BulletController bullet2 = Instantiate(bullet, position, Quaternion.identity);
        bullet2.Initialize(new Vector2(bulletSpeed * facing, 0), false);
		shoot.PlayOneShot (shotSound);
    }
}

public enum DashState
{
    Ready,
    Dashing,
    Cooldown
}