using UnityEngine;
using System.Collections;

[RequireComponent(typeof(Animator))]
public class PlatformController : MonoBehaviour {

	[Tooltip("Platform Animation Speed")]
	public float speed;

	[Tooltip("Dictates which platform animation is played.")]
	public Pattern pattern;

	[Tooltip("If true, will only animate when player collides.")]
	public bool isTriggerable = false;

	[Tooltip("Reverse direction of the animation.")]
	public bool reverseLoop = false;

	//Cached variables.
	private Animator animator;

	public enum Pattern
	{
		VERTICAL = 1,
		HORIZONTAL = 2,
		DIAGONAL = 3,
		CIRCLE = 4
	};
			
	void Start () 
	{
		//Set all the platform animation values.
		animator = GetComponent<Animator> ();
		animator.SetInteger ("Pattern", (int)pattern);
		animator.SetFloat ("Speed", speed);
		animator.SetBool ("Triggered", !isTriggerable);
		animator.SetBool ("LoopReverse", reverseLoop);
	}		

	//On player trigger.
	public void SetTrigger(bool trigger)
	{
		//True if always animating, or set to triggerable and passed in true.
		animator.SetBool ("Triggered", (!isTriggerable) || (isTriggerable && trigger));
	}
}
