using System.Collections;
using System.Collections.Generic;
using UnityEngine;




public class Lava : MonoBehaviour
{

    //private bool TouchingPlayer = false;
    // Use this for initialization
    void OnTriggerEnter2D(Collider2D other)
    {
        SpriteRenderer renderer = GetComponent<SpriteRenderer>();
        if (other.gameObject.name == "Player")
        {
            //TouchingPlayer = false;
			PlayerController player = other.gameObject.GetComponent<PlayerController>();
			player.Die();

		}

        }
    }