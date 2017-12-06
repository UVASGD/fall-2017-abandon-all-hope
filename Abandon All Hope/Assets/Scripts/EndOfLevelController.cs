using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class EndOfLevelController : MonoBehaviour {



	public string levelToLoad = "Boss2";

	//private bool TouchingPlayer = false;
	// Use this for initialization
	void Start () {
		

	}

	// Update is called once per frame
	void Update () {



	}


	void OnTriggerEnter2D(Collider2D other)
	{
		if (other.gameObject.name == "Player")
		{
			print ("aasdf");
			//PlayerController player = other.gameObject.GetComponent<PlayerController>();
			//player.setHealth (player.Health + 10); //resets player health for next level
			Application.LoadLevel(levelToLoad);
			//Destroy (this.gameObject);
		}
	}
}
