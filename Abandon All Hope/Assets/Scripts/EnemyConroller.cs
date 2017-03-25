using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyConroller : MonoBehaviour {
    const int LEFT = -1;
    const int RIGHT = 1;

    public float speed = 20;
    public float jumpspeed = 1000;
    public bool isstationary = false;
    public float walkRange = 0;
    public float pause = 1.0f;
    public int maxHealth = 5;
    public int power = 1;

    private int facing = LEFT;
    private float leftposition;
    private float rightposition;
    private bool waiting;
    private int health;

    // Use this for initialization
    void Start () {
        health = maxHealth;
        rightposition = transform.position.x;
        leftposition = rightposition - walkRange;
    }

    // Update is called once per frame
    void Update () {
        if (!isstationary)
        {
            Rigidbody2D body = GetComponent<Rigidbody2D>();
            if (!waiting)
            {
                if (body.position.x >= rightposition && facing == RIGHT || 
                    body.position.x <= leftposition && facing == LEFT)
                {
                    StartCoroutine(WaitAndChangeDirection(pause));
                }
                else
                {
                    body.AddForce(Vector2.right * facing * speed);
                    //body.velocity = Vector2.right * facing * speed;
                }
            }
            SpriteRenderer sprite = GetComponent<SpriteRenderer>();
            sprite.flipX = facing == LEFT;
        }
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
        int hits = GetComponent<Rigidbody2D>().Cast(Vector2.down, new RaycastHit2D[1], 0.02f);
        return hits > 0;
    }
}
