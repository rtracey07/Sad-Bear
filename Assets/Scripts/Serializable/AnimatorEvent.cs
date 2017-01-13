using UnityEngine;
using System.Collections;

[System.Serializable]
public class AnimatorEvent {

	[Header("Enter Info (Leave Empty to Ignore):")]
	public string enterStateName;
	public bool enterStateValue;

	[Header("Exit Info (Leave Empty to Ignore):")]
	public string exitStateName;
	public bool exitStateValue;

	[Header("Select Info (Leave Empty to Ignore):")]
	public string selectStateName;
	public bool selectStateValue;

	[Header("Scene State Change Info:")]
	public bool loadScene;
	public string sceneName;

	[Tooltip("Quit Game if Selected")]
	public bool quit;

	[Header("Target Animator:")]
	public Animator animator;
}
