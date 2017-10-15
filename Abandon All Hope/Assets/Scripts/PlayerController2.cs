using System.Collections;
using System.Collections.Generic;
using UnityEngine.SceneManagement;
using UnityEngine;

public class PlayerController2 : MonoBehaviour {
	const int LEFT = -1, RIGHT = 1;

	int prev_dir = 1;

	public float speed;
	public float jumpspeed;
	public float sprintspeed;
	public float midairaccel;
	public int maxhealth = 10;
	//public Vector2 jumpHeight;

	public BulletController bullet;
	public float bulletSpeed = 4;

	// private bool grounded = false;
	private int facing = 1;
	public int health;

	private float old_pos;

	private int sprintFrames = 0;
	private int framesUntilSprint = 90;
	private int jumpTimer = 0;
	private int jumpCooldown = 5; //number of frames before you can jump again. Used to prevent multiple jumps triggering in the space of one jump.

	public Animator anim;

	private AudioSource shoot;
	public AudioClip shotSound;
	private Rigidbody2D body;

	// Use this for initialization
	void Start () {
		anim.speed = 2;
		health = maxhealth;
		old_pos = transform.position.x;
		shoot = GetComponent<AudioSource>();
		body = GetComponent<Rigidbody2D>();
	}

	// Update is called once per frame
	void Update () {
		anim.enabled = true;
		print ("sprintFrames: " + sprintFrames);
		if(old_pos == transform.position.x && prev_dir ==facing)
		{
			sprintFrames = 0;
			print("still");
			anim.enabled = false;
		}
		if (CheckGrounded ()) {
			print ("still on the ground");
			if (Input.GetKey (KeyCode.LeftArrow) && Input.GetKey (KeyCode.RightArrow)) {
				body.velocity = new Vector2 (0, 0);
			} else if (Input.GetKey (KeyCode.LeftArrow)) {
				sprintFrames++;
				facing = LEFT;
				if (sprintFrames >= framesUntilSprint) {
					body.velocity = new Vector2 (-sprintspeed, 0);
				} else {
					body.velocity = new Vector2 (-speed, 0);
				}
			} else if (Input.GetKey (KeyCode.RightArrow)) {
				sprintFrames++;
				facing = RIGHT;
				if (sprintFrames >= framesUntilSprint) {
					body.velocity = new Vector2 (sprintspeed, 0);
				} else {
					body.velocity = new Vector2 (speed, 0);
				}
			} else {
				body.velocity = new Vector2 (0, 0);
			}
			if (Input.GetKey (KeyCode.UpArrow)) {
				sprintFrames = 0;
				jumpTimer = jumpCooldown;
				body.AddForce (new Vector2 (0, 30), ForceMode2D.Impulse);	
			}
		} else {
			print ("not on the ground");
			if (Input.GetKey (KeyCode.LeftArrow) && Input.GetKey (KeyCode.RightArrow)) {
			} else if (Input.GetKey (KeyCode.LeftArrow)) {
				facing = LEFT;
				body.AddForce (new Vector2(-midairaccel, 0));
			} else if (Input.GetKey (KeyCode.RightArrow)) {
				facing = RIGHT;
				body.AddForce (new Vector2(midairaccel, 0));
			} else {
			}
		}
//		if(!CheckGrounded()){
//			print("midair");
//			anim.enabled = false;
//		}
//		if (Input.GetKey(KeyCode.LeftArrow))
//		{
//			if(CheckGrounded())
//			{
//				sprintFrames++;
//			}
//
//			if (sprintFrames >= 90)
//			{
//				body.AddForce(Vector2.left * (speed+15));
//				print("Sprinting");
//			}
//			else
//			{
//				body.AddForce(Vector2.left * speed);
//			}
//
//			facing = LEFT;
//
//		}
//		if (Input.GetKey(KeyCode.RightArrow))
//		{
//			if (CheckGrounded())
//			{
//				sprintFrames++;
//			}
//			if (sprintFrames >= 90)
//			{
//				body.AddForce(Vector2.right * (speed + 15));
//				print("Sprinting");
//			}
//			else
//			{
//				body.AddForce(Vector2.right * speed);
//			}
//			facing = RIGHT;
//		}
//		if (Input.GetKey (KeyCode.UpArrow) && CheckGrounded () && jumpTimer == 0) {
//			//updated jump - maybe less floaty now? -Susannah
//			jumpTimer = jumpCooldown;
//			GetComponent<Rigidbody2D> ().AddForce (new Vector2 (0, 30), ForceMode2D.Impulse);	
//			// body.AddForce(Vector2.up * jumpspeed);
//			// sprintFrames = 0;
//		}
		if (jumpTimer > 0) {
			jumpTimer--;
		}
		if (Input.GetKeyDown(KeyCode.Space)) {
			Shoot();
		}
		SpriteRenderer sprite = GetComponent<SpriteRenderer>();
		sprite.flipX = facing == LEFT;
		if (!CheckGrounded() && body.position.y < -25)
		{
			Die();
		}

		old_pos = transform.position.x;
		prev_dir = facing;
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
		shoot.PlayOneShot (shotSound);
	}
}
//
//public enum DashState
//{
//	Ready,
//	Dashing,
//	Cooldown
//}