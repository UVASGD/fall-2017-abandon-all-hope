using System;

public struct ScreenTransitionParams
{
	public float fadeInTime;	// amount of time to fade into the screen
	public float waitTime;		// amount of time to wait after fade in completes
	public float fadeOutTime;	// amount of time for text to fade out
	public string text;			// text displayed during transition
	public int fontSize;		// how large font for text should be
	// transition function for how quickly to fade in?
	// other params as needed
}
