using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIController : MonoBehaviour {

    public Slider healthbar;
	public Text livesIndicator;

    private PlayerController player;

	// Use this for initialization
	void Start () {
		player = GameObject.Find("Player").GetComponent<PlayerController>();
    }
	
	// Update is called once per frame
	void Update () {
        healthbar.maxValue = player.maxhealth;
        healthbar.value = player.Health;
	}
	public void changeLivesIndicator (){
        livesIndicator.text = player.Lives.ToString();
	}
}
