using UnityEngine;
using System.Collections;

[RequireComponent(typeof(Animator))]
//[RequireComponent(typeof(Rigidbody))]
public class Enemy : Damagable {

	[Tooltip("Enemy Move Speed.")]
	public float moveSpeed = 1.0f;

	[Tooltip("Length Enemy is stunned after being hit.")]
	public float freezeTime = 1.0f;

	[Tooltip("Allow the Enemy to advance towards player.")]
	public bool moveable;

	[Tooltip("Renderer for the Enemy's model")]
	public SkinnedMeshRenderer meshRenderer;

	[Tooltip("Enemy can move upward")]
	public bool hasUpwardThrust;

	[Tooltip("Upward thrust value")]
	public float thrust;

	[Tooltip("Toggle whether enemies can be destroyed when too far away.")]
	public bool destroyAtDistance;

	[Tooltip("distance at which enemy is destroyed.")]
	public float distanceThreshold;

	[HideInInspector]
	public bool isSad = false;

	//Cached Variables.
	private float time = 0.0f;
	private Transform player;
	private Vector3 distance;
	private Animator animator;
	private Rigidbody body;
	private ProjectileController projectile;
	private MaterialPropertyBlock matProperties;
	private Color color;

	void Start()
	{
		//Cache used variables.
		player = GameObject.FindGameObjectWithTag ("Player").transform;
		projectile = GetComponent<ProjectileController> ();
		animator = GetComponent<Animator> ();
		body = GetComponent<Rigidbody> ();

		//Setup a property block for modifying shader properties
		//without duplicating the material in memory.
		matProperties = new MaterialPropertyBlock ();

		//Grab the current material's _Color parameter.
		color = meshRenderer.sharedMaterial.color;

		//Attach the property block to the renderer.
		meshRenderer.SetPropertyBlock (matProperties);
	}
		
	void Update () {

		//Update time.
		time += Time.deltaTime;

		//Vector colinear to vector between player and enemy.
		distance = player.position - transform.position;

		//Check to see if enemy is too far from the player.
		if (destroyAtDistance && distance.magnitude > distanceThreshold) 
		{
			Destroy (this.gameObject);
		}

		//Only move if not in the sad state.
		if (!isSad) 
		{
			Rotate ();

			//Only move if character is able to (i.e. not a turret).
			if (moveable) 
			{
				Move ();
			}

			//Fire the enemy's projectile.
			if (projectile) 
			{
				projectile.FireProjectile (distance.magnitude, player.position.y, ref time);
			}
		}
	}

	//Turn enemy towards the player.
	void Rotate()
	{
		transform.rotation = Quaternion.Slerp(transform.rotation, 
			Quaternion.LookRotation(distance), 60 * Time.deltaTime);
	}

	//Move enemy.
	void Move()
	{
		//Enemy always moves forward.
		transform.position += transform.forward * moveSpeed * Time.deltaTime;

		if (hasUpwardThrust && transform.position.y < player.position.y && body.velocity.y < thrust) 
		{
			body.AddForce (new Vector3(0,thrust,0));
		}

		//Set movement animation.
		animator.SetFloat ("Speed", moveSpeed);
	}

	//Update enemy health.
	override public void ChangeHealth(float difference)
	{
		//Enemy is always one-hit. If not sad, fade to sadness.
		if (!isSad) 
		{
			StartCoroutine (FadeToGray (2.0f, 1.0f, 0.0f));
		}
	}

	//Set the Enemy to Gray.
	IEnumerator FadeToGray(float time, float startAlpha, float endAlpha)
	{
		float currTime = 0.0f;

		//Set the Enemy to Sad, stopping it's movement.
		isSad = true;

		//Start the sad Animation.
		animator.SetBool ("Sad", isSad);

		//Fade the material's alpha value.
		do 
		{
			color.a =  Mathf.Lerp(startAlpha, endAlpha, currTime/time);
			matProperties.SetColor("_Color", color);
			meshRenderer.SetPropertyBlock(matProperties);
			currTime += Time.deltaTime;
			yield return null;
		} 
		while(currTime <= time);

		//Pause in sadness for the allotted time.
		yield return new WaitForSeconds (freezeTime);

		//Reset Sad.
		currTime = 0.0f;
		isSad = false;
		animator.SetBool ("Sad", isSad);

		//Fade the alpha back to the original.
		do 
		{
			color.a =  Mathf.Lerp(endAlpha, startAlpha, currTime/time);
			matProperties.SetColor("_Color", color);
			meshRenderer.SetPropertyBlock(matProperties);
			currTime += Time.deltaTime;
			yield return null;
		} 
		while(currTime <= time);
	}
}
