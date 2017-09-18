using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyConroller : MonoBehaviour {
    const int LEFT = -1;
    const int RIGHT = 1;

    public float speed = 20;
    public float jumpspeed = 1000;
    public bool stationary = false;
    public bool followPlayer = true;
    public float walkRange = 0;
    public float visionRange = 0;
    public Vector2 shootRange;
    public float pause = 1.0f;
    public int maxHealth = 5;
    public int power = 1;
    public float shootCooldown = 1;

    public BulletController bullet;
    public float bulletSpeed = 4;

    public GameObject deathFX;

    private int facing = LEFT;
    private float leftPosition;
    private float rightPosition;
    private bool waiting = false;
    private float shootTimer = 0;
    private int health;

    // Use this for initialization
    void Start () {
        health = maxHealth;
        rightPosition = transform.position.x;
        leftPosition = rightPosition - walkRange;
    }

    // Update is called once per frame
    void Update () {
        Rigidbody2D body = GetComponent<Rigidbody2D>();
        GameObject player = GameObject.Find("Player");
        shootTimer -= Time.deltaTime;
        if (shootTimer <= 0 && InShootRange(player))
        {
            Shoot();
            shootTimer = shootCooldown;
        }
        if (followPlayer && DetectPlayer(player))
        {
            waiting = false;
            body.AddForce(Vector2.right * facing * speed);
        }
        else if (!stationary)
        {
            if (!waiting)
            {
                if (body.position.x >= rightPosition && facing == RIGHT || 
                    body.position.x <= leftPosition && facing == LEFT)
                {
                    StartCoroutine(WaitAndChangeDirection(pause));
                }
                else
                {
                    body.AddForce(Vector2.right * facing * speed);
                    //body.velocity = Vector2.right * facing * speed;
                }
            }
        }
        SpriteRenderer sprite = GetComponent<SpriteRenderer>();
        sprite.flipX = facing == LEFT;
    }

    IEnumerator WaitAndChangeDirection(float seconds)
    {
        waiting = true;
        yield return new WaitForSeconds(seconds);
        if (waiting)
        {
            facing = -facing;
            waiting = false;
        }
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
        //print(name + " died");
        //if (deathFX) {
            GameObject deathFXOBJ = (GameObject)Instantiate(deathFX, transform.position, deathFX.transform.rotation);
            Destroy(deathFXOBJ, deathFXOBJ.GetComponent<ParticleSystem>().startLifetime);
        //}
        Destroy(gameObject);
    }

    void OnCollisionEnter2D(Collision2D other)
    {
        if (other.gameObject.tag == "Player")
        {
            other.gameObject.GetComponent<PlayerController>().Hit(power);
        }
    }

    private bool CheckGrounded()
    {
        return GetComponent<Rigidbody2D>().Cast(Vector2.down, new RaycastHit2D[1], 0.02f) > 0;
    }

    private bool DetectPlayer(GameObject player)
    {
        Vector3 disp = player.transform.position - transform.position;
        return disp.x * facing >= 0 && disp.sqrMagnitude <= visionRange * visionRange;
    }

    private bool InShootRange(GameObject player)
    {
        Vector3 disp = player.transform.position - transform.position;
        disp.x *= facing;
        return disp.x >= 0 && disp.x <= shootRange.x && Mathf.Abs(disp.y) <= shootRange.y;
    }

    private void Shoot()
    {
        Vector3 position = transform.position + new Vector3(.7f * facing, 0f);
        BulletController bullet2 = Instantiate(bullet, position, Quaternion.identity);
        bullet2.Initialize(new Vector2(bulletSpeed * facing, 0), true);
    }

}
