using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ScreenTransition : MonoBehaviour {
	private int state = -1;
	private float timer = 0f;
	
	private ScreenTransitionParams mParams;
	private Action mOnComplete;

	// Use this for initialization
	void Start () {
	}
	
	// Update is called once per frame
	void Update () {
		if (state >= 0) {
			// update timer
			timer += Time.deltaTime;

			// grab children components that are needed later
			Image bg = gameObject.transform.FindChild ("BG").GetComponent<Image> ();
			Text text = gameObject.transform.FindChild ("Text").GetComponent<Text> ();

			// state functions and transitions
			switch (state) {
			case 0:
				bg.color = new Color(bg.color.r, bg.color.g, bg.color.b, TransitionFunc(timer, mParams.fadeInTime));
				text.color = new Color(text.color.r, text.color.g, text.color.b, TransitionFunc(timer, mParams.fadeInTime));
				if (timer > mParams.fadeInTime) {
					timer = 0f;
					state = 1;
				} 
				break;
			case 1:
				if (timer > mParams.waitTime) {
					timer = 0f;
					state = 2;
				} 
				break;
			case 2:
				text.color = new Color(text.color.r, text.color.g, text.color.b, 1f - TransitionFunc(timer, mParams.fadeInTime));
				if (timer > mParams.fadeOutTime) {
					timer = 0f;
					state = -1;
					if (mOnComplete != null) {
						mOnComplete ();
					}
				}
				break;
			}
		}
	}

	private float TransitionFunc(float timeDone, float timeToComplete) {
		if (timeToComplete <= 0) {
			return 1.0f;	// already done
		}
		// linear
		return Mathf.Clamp (timeDone / timeToComplete, 0.0f, 1.0f);
	}

	private void Commence(ScreenTransitionParams p, Action onComplete) {
		this.mParams = p;
		this.mOnComplete = onComplete;

		// initialize text child
		Text text = gameObject.transform.FindChild ("Text").GetComponent<Text> ();
		text.fontSize = mParams.fontSize;
		text.text = mParams.text;

		// bring to front
		transform.SetSiblingIndex (transform.parent.childCount - 1);

		this.state = 0;	// updates will act as state machine
	}

	/// <summary>
	/// Begin a screen transition and execute a callback when complete
	/// </summary>
	/// <param name="p">parameters for the screen transition</param>
	/// <param name="onComplete">Function to call when transition is complete</param>
	public static void DoTransition(ScreenTransitionParams p, Action onComplete) {
		GameObject.FindGameObjectWithTag ("ScreenTransition").GetComponent<ScreenTransition> ().Commence(p, onComplete);
	}
}
