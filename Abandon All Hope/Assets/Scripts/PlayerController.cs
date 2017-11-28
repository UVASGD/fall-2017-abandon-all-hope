using System.Collections;
using System.Collections.Generic;
using UnityEngine.SceneManagement;
using UnityEngine;

public class PlayerController: MonoBehaviour {
    const int LEFT = -1, RIGHT = 1;
    const float RESPAWN_DAMAGE_COOLDOWN = 1.5f;
    int prev_dir = 1;

    //private GameObject platformList;
    private Transform lastPlatform;
    public float speed;
    public float jumpspeed;
    public float sprintspeed;
    public float midairaccel;
    public float jumpVelDampen;
    public int maxhealth = 10;
    public int maxLives = 3;
    public bool invincible = false;
    private float rspwnHitCldwn = 0;
    //public Vector2 jumpHeight;
    private GameObject uiController;

    public float bulletSpeed = 15;

    // private bool grounded = false;
    private int facing = 1;
    public int health;
    private int lives;

    private int sprintFrames = 0;
    private int framesUntilSprint = 70;
    private int jumpTimer = 0;
    private int jumpCooldown = 3; //number of frames before you can jump again. Used to prevent multiple jumps triggering in the space of one jump.

    private Animator anim;
    private Transform shoot_loc;
    private float shoot_offx;
    private AudioSource audioSrc;
    private Rigidbody2D body;
    private SpriteRenderer sprite;
    private GameObject respawnFX;
    public BulletController bullet;

    // Use this for initialization
    void Start() {
        health = maxhealth;
        lives = maxLives;
        //platformList = GameObject.Find("");
        audioSrc = GetComponent<AudioSource>();
        body = GetComponent<Rigidbody2D>();
        sprite = GetComponent<SpriteRenderer>();
        anim = GetComponent<Animator>();
        shoot_loc = transform.FindChild("Shoot_Loc");
        shoot_offx = shoot_loc.localPosition.x;

        GameObject b = Resources.Load<GameObject>("Prefabs/Bullet");
        bullet = b.GetComponent<BulletController>();
        uiController = GameObject.Find("UI Controller");
        respawnFX = Resources.Load<GameObject>("Prefabs/Respawn");
    }

    // Update is called once per frame
    void Update() {
        if (rspwnHitCldwn > 0) {
            sprite.color = new Color(1, 1, 1, 1-rspwnHitCldwn/RESPAWN_DAMAGE_COOLDOWN);
            rspwnHitCldwn -= Time.deltaTime;
        } else {
            sprite.color = Color.white;
            HandleInput();
        }


        // update animation
        anim.SetBool("OnGround", CheckGrounded());
        anim.SetBool("Moving", Mathf.Abs(body.velocity.x) > 0);
        anim.SetBool("Dashing", (sprintFrames >= framesUntilSprint));
        sprite.flipX = (facing == LEFT);
        Debug.LogWarning("Shoot Loc: " + shoot_loc);
        shoot_loc.localPosition = new Vector3(facing * shoot_offx, 0, 0);
    }

    private void HandleInput() {
        Vector2 move = new Vector2(Input.GetAxis("Horizontal"), Input.GetAxis("Vertical"));
        if (CheckGrounded()) {
            if (Mathf.Abs(move.x) > 0) {
                sprintFrames++;
                facing = move.x > 0 ? RIGHT : LEFT;
                if (sprintFrames >= framesUntilSprint)
                    body.velocity = new Vector2(sprintspeed * move.x, 0);
                else
                    body.velocity = new Vector2(speed * move.x, body.velocity.y);
            } else {
                sprintFrames = 0;
                body.velocity = new Vector2(0, body.velocity.y);
            }

            if (move.y > 0 && jumpTimer == 0) {
                sprintFrames = 0;
                jumpTimer = jumpCooldown;
                body.AddForce(new Vector2(0, jumpspeed), ForceMode2D.Impulse);
                body.AddForce(new Vector2(body.velocity.x * jumpVelDampen, 0));
                audioSrc.PlayOneShot(Resources.Load<AudioClip>("Sounds/jump"));
                // add jetpack particles
            }
        } else {
            if (Mathf.Abs(move.x) > 0) {
                facing = move.x > 0 ? RIGHT : LEFT;
                body.AddForce(new Vector2(midairaccel * move.x, 0));
            }
        }

        if (jumpTimer > 0) jumpTimer--;
        if (!CheckGrounded() && body.position.y < -25)
            Die();

        prev_dir = facing;
        if (Input.GetButtonDown("Jump"))
            Shoot();
    }

    public int Health {
        get { return health; }
    }

    public void setHealth(int health) {
        this.health = health;
    }

    public int Lives {
        get { return lives; }
    }
    public void setLives(int lives) {
        this.lives = lives;
    }

    private bool CheckGrounded() {
        RaycastHit2D[] thingIHit = new RaycastHit2D[1];
        bool grounded = GetComponent<Rigidbody2D>().Cast(Vector2.down, thingIHit, 0.02f) > 0;
        if (grounded) {
            string tag = thingIHit[0].transform.gameObject.tag;
            if (tag.Equals("Platform")) {
                lastPlatform = thingIHit[0].transform;
            }
        }

        return grounded;
    }

    private void Respawn() {
        body.position = new Vector2(lastPlatform.position.x, 
            lastPlatform.position.y + (lastPlatform.localScale.y));

        if (respawnFX) {
            GameObject respawnFXOBJ = Instantiate(respawnFX, transform.position, Quaternion.identity);
            respawnFXOBJ.transform.parent = this.transform;
        }

        //TODO: respawn animation stuff
        // 	-fade in player
        // 	-disable movement for a tiny bit? 

        //	-invuln period?
        rspwnHitCldwn = RESPAWN_DAMAGE_COOLDOWN;
        return;
    }

    public void Hit(int damage) {
        if (invincible || rspwnHitCldwn>0) return;
        health -= damage;
        if (health <= 0)
            Die();
    }

    private void Die() {
        print(name + " died!!1");
        if (lives <= 0) {
            SceneManager.LoadScene("death screen");
        } else {
            Respawn();
            lives--;
            health = maxhealth;
            uiController.GetComponent<UIController>().changeLivesIndicator();
        }
    }

    private void Shoot() {
        anim.SetTrigger("Shoot");
        Vector2 position = shoot_loc.position;
        BulletController bullet2 = Instantiate(bullet, position, Quaternion.identity);
        bullet2.Initialize(new Vector2(bulletSpeed * facing, 0), false);
        audioSrc.PlayOneShot(Resources.Load<AudioClip>("Sounds/laser_shoot"));
    }
}