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
    [Tooltip("Root of UI for credits")]
    public GameObject creditsScreen;

	// Use this for initialization
	void Start () {
	}
	
	// Update is called once per frame
	void Update () {		
	}

    public void StartGame()
    {
        ScreenTransition.DoTransition(new ScreenTransitionParams()
        {
            fadeInTime = 2.0f,
            waitTime = 7.0f,
            fadeOutTime = 2.0f,
            text = "Through me you go to the grief wracked city; \n Through me you go to everlasting pain; Through me you go a pass among lost souls. \n Justice inspired my exalted Creator: I am a creature of the Holiest Power, of Wisdom in the Highest and of Primal Love. \n Nothing till I was made was made, only eternal beings. \n And I endure eternally. \n Abandon all hope — Ye Who Enter Here,     \n Dante's Inferno: Canto III",

            fontSize = 25
        }, () =>
        {
            SceneManager.LoadScene(mainScene);
        });
    }

	// shows the "top level" title screen buttons
	public void ShowTitle() {
		titleScreen.SetActive (true);
		controlsScreen.SetActive (false);
        creditsScreen.SetActive(false);
	}

	// shows the controls instructions layout
	public void ShowControls() {
		controlsScreen.SetActive (true);
		titleScreen.SetActive (false);
        creditsScreen.SetActive(false);
	}
    public void ShowCredits()
    {
        controlsScreen.SetActive(false);
        titleScreen.SetActive(false);
        creditsScreen.SetActive(true);

    }

    public void RestartLevel()
    {
        SceneManager.LoadScene("main");
    }
}
