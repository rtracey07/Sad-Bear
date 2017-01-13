using UnityEngine;
using System.Collections;

[System.Serializable]
public class LevelEvent {

	[Header("Dialog Box Info:")]
	[Tooltip("Text message to appear onscreen during event.")]
	public string dialog;

	[Range(10.0f,80.0f)]
	public int fontSize;

	[Tooltip("Delay before event begins")]
	public float startDelay;

	[Tooltip("Toggle text fade-in and fade-out.")]
	public bool fade;

	[Tooltip("Fade-in duration.")]
	public float fadeIn;

	[Tooltip("Pause when message is on-screen.")]
	public float duration;

	[Tooltip("Fade-out duration.")]
	public float fadeOut;



	[Header("Player Handling:")]
	[Tooltip("Automatically start next event in list.")]
	public bool autoContinue;

	[Tooltip("Freeze Character in position during event")]
	public bool pauseCharacterControl;

	[Tooltip("Respawn the character at the spawn point.")]
	public bool respawn;



	[Header("Scene Transition:")]
	[Tooltip("Load into a new scene after event completion")]
	public bool loadScene;
	[Tooltip("Next scene")]
	public string sceneName;
}
