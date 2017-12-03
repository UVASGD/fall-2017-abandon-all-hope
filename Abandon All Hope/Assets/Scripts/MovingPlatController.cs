using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MovingPlatController : MonoBehaviour {
    private GameObject target = null;
    private Vector3 offset ;
    private int steps = 0;
    public int maxsteps;
    public float xvel; //velocity in x direction
    public float yvel;
    // Use this for initialization
    void Start() {
        target = null;
    }
     void OnTriggerEnter2D(Collider2D col)
    {
        //to get this to work, you're going to need two moving platforms- one that has the
        //trigger box checked, the other doesn't. Both of them need to have the same starting positions
        //velocities, etc. It's not a pretty fix but it gets the job done.
       
            target = col.gameObject;
            offset = target.transform.position - transform.position;
        
        
        }
    void OnTriggerStay2D(Collider2D col)
    {
        
        //target = col.gameObject;
            offset = target.transform.position - transform.position;
            //this is supposed to make sure that the player moves with the platform
           // print("stay is active");
        
    }
    void OnTriggerExit2D(Collider2D col)
    {
        target = null;
        //print("exit is active");
    }
    // Update is called once per frame


    private void Update()
    {
        if (steps == maxsteps)
        {
            xvel *= -1;
            yvel *= -1;
            steps = 0;
            
        }
        transform.position = new Vector3(transform.position.x+xvel, transform.position.y+yvel);

        //offset = target.transform.position - transform.position;
        steps += 1;
    }
    void LateUpdate()
    {


        if (target != null)
        {
            target.transform.position = transform.position + offset;
        }

    }
   
}