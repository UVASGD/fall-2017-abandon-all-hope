using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class TitleUIManager : MonoBehaviour {

	[Tooltip("The name of the scene that \"Play\" redirects to")]
	public string mainScene;

	[Tooltip("The root of UI specific to the top level title screen buttons")]
	public GameObject titleScreen;
	[Tooltip("The root of UI specific to controls instructions")]
	public GameObject controlsScreen;

	// Use this for initialization
	void Start () {
	}
	
	// Update is called once per frame
	void Update () {		
	}

	public void StartGame() {
		ScreenTransition.DoTransition(new ScreenTransitionParams() { 
			fadeInTime=1.0f,
			waitTime = 2.0f,
			fadeOutTime = 1.0f,
			text="O HAI",
			fontSize=40
		}, () => {
			SceneManager.LoadScene (mainScene);
		});
	}

	// shows the "top level" title screen buttons
	public void ShowTitle() {
		titleScreen.SetActive (true);
		controlsScreen.SetActive (false);
	}

	// shows the controls instructions layout
	public void ShowControls() {
		controlsScreen.SetActive (true);
		titleScreen.SetActive (false);
	}
}
