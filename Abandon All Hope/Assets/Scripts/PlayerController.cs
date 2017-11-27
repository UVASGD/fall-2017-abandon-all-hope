using System.Collections;
using System.Collections.Generic;
using UnityEngine.SceneManagement;
using UnityEngine;

namespace PC {


    public class PlayerController : MonoBehaviour {
        const int LEFT = -1, RIGHT = 1;

        int prev_dir = 1;

        public GameObject platformList;
        private int lastPlatformID;
        public float speed;
        public float jumpspeed;
        public float sprintspeed;
        public float midairaccel;
        public float jumpVelDampen;
        public int maxhealth = 10;
        public int maxLives = 3;
        //public Vector2 jumpHeight;
        public GameObject uiController;


        public BulletController bullet;
        public float bulletSpeed = 15;

        // private bool grounded = false;
        private int facing = 1;
        private int health;
        private int lives;

        private float old_pos;

        private int sprintFrames = 0;
        private int framesUntilSprint = 70;
        private int jumpTimer = 0;
        private int jumpCooldown = 3; //number of frames before you can jump again. Used to prevent multiple jumps triggering in the space of one jump.

        public Animator anim;

        private AudioSource shoot;
        public AudioClip shotSound;
        private Rigidbody2D body;

        public GameObject respawnFX;
        //	public Sprite idleSprite;
        //	public Sprite jumpSprite;
        //
        // Use this for initialization
        void Start() {
            anim.speed = 2;
            health = maxhealth;
            lives = maxLives;
            old_pos = transform.position.x;
            shoot = GetComponent<AudioSource>();
            body = GetComponent<Rigidbody2D>();
        }

        // Update is called once per frame
        void Update() {
            anim.enabled = true;
            if (old_pos == transform.position.x && prev_dir == facing) {
                //            sprintFrames = 0;
                //            print("still");
                anim.enabled = false;
            }
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
            if (Input.GetKeyDown(KeyCode.Space)) {
                Shoot();
            }
            SpriteRenderer sprite = GetComponent<SpriteRenderer>();
            sprite.flipX = facing == LEFT;
            if (!CheckGrounded() && body.position.y < -25) {
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
            if (health <= 0)
                Die();
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
                PlatformID platformIDComponent = thingIHit[0].transform.gameObject.GetComponent<PlatformID>();
                if (platformIDComponent != null) {
                    lastPlatformID = platformIDComponent.platformID;
                }
            }
            return grounded;
            //Bounds bounds = GetComponent<Collider2D>().bounds;
            //RaycastHit2D hit = Physics2D.BoxCast(bounds.center, bounds.size, 0, Vector2.down, 0.02f);
            //return grounded = hit.collider != null;
        }

        public void Hit(int damage) {
            //print(name + " hit: " + damage + " damage");
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
            Vector2 position = transform.position + new Vector3(.6f * facing, 0f);
            BulletController bullet2 = Instantiate(bullet, position, Quaternion.identity);
            bullet2.Initialize(new Vector2(bulletSpeed * facing, 0), false);
            shoot.PlayOneShot(shotSound);
        }

        private void Respawn() {
            foreach (Transform platform in platformList.transform) {
                //			print ("current platform's ID: " + platform.gameObject.GetComponent<PlatformID> ().platformID);
                if (platform.gameObject.GetComponent<PlatformID>().platformID == lastPlatformID) {
                    body.position = new Vector2(platform.position.x, platform.position.y + (platform.localScale.y));
                    if (respawnFX) {
                        GameObject respawnFXOBJ = Instantiate(respawnFX, transform.position, Quaternion.identity);
                        respawnFXOBJ.transform.parent = this.transform;
                    }
                    //TODO: respawn animation stuff
                    // 	-fade in player
                    // 	-disable movement for a tiny bit? 
                    //	-invuln period?
                    return;
                }
            }
        }
    }

    public enum DashState {
        Ready,
        Dashing,
        Cooldown
    }
}