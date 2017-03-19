using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyConroller : MonoBehaviour {
    public float speed = 10;
    public float jumpspeed = 100;

    private bool grounded = false;
    private int facingRight = 1;

    // Use this for initialization
    void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}

    private void CheckGrounded()
    {
        Bounds bounds = GetComponent<Collider2D>().bounds;
        RaycastHit2D hit = Physics2D.BoxCast(bounds.center, bounds.size, 0, Vector2.down, 0.02f);
        grounded = hit.collider != null;
    }
}
