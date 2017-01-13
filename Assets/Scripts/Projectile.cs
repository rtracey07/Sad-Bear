using UnityEngine;
using System.Collections;

/** Projectile Game Object for doing damage to given targets.  */
public class Projectile : MonoBehaviour {

	[Header("Damage and Target:")]
	[Tooltip("Amount of damage done to damagable object.")]
	public float damage;

	[Tooltip("Target tagged objects to damage")]
	public string target;

	[Header("Lifetime and Size:")]
	[Tooltip("How long the projectile exists.")]
	public float lifetime;

	[Tooltip("Rate at which the object goes from 0 scale to given scale")]
	public float fadeIn;

	[Tooltip("Rate at which the object goes from given scale to 0 scale")]
	public float fadeOut;

	[Tooltip("Final scale of projectile")]
	public Vector3 scale;

	//Cached variables.
	private float time;
	private bool isDestroyed = false;

	void Start()
	{
		//Initialize Coroutine on instantiation.
		StartCoroutine (Fade(fadeIn, Vector3.zero, scale, false));
	}

	void Update()
	{
		time += Time.deltaTime;

		//Check if object is ready to fade out.
		if (time > lifetime-fadeOut && !isDestroyed) 
		{
			isDestroyed = true;

			//Fade out and destroy.
			StartCoroutine (Fade(fadeOut, transform.localScale, Vector3.zero, true));
		}
	}

	//Check for collision with given target objects.
	void OnTriggerEnter(Collider other){

		//Collision with target.
		if (other.tag == target) 
		{
			//Modify target's health by the amount of damage.
			other.gameObject.GetComponent<Damagable> ().ChangeHealth (-damage);

			//Destroy the projectile.
			Destroy (this.gameObject);
		} 
		//Collision with another projectile.
		else if (other.tag == "Projectile") 
		{
			//One projectile dies.
			Destroy (other.gameObject);
		}
	}

	//Coroutine for increasing / decreasing scale over time.
	IEnumerator Fade(float time, Vector3 startScale, Vector3 endScale, bool destroy)
	{
		float currTime = 0.0f;

		//Scale object over time.
		do {
			transform.localScale = Vector3.Lerp(startScale, endScale, currTime/time);
			currTime += Time.deltaTime;
			yield return null;
		} 
		while(currTime <= time);

		//Destroy object on completion, if selected.
		if (destroy) 
		{
			Destroy (this.gameObject);
		}
	}
}
