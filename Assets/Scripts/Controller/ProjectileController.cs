using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class ProjectileController : MonoBehaviour {

	[Tooltip("Rate of projectile fire")]
	public float fireRate = 1.0f;

	[Tooltip("Range of the projectile")]
	public float fireRange;

	[Tooltip("How far the target's height offest can be")]
	public float vertRange;

	[Tooltip("Rate at which projectile travels forward")]
	public float projectileSpeed = 1.0f;

	[Tooltip("Object being fired.")]
	public GameObject projectile;

	[Tooltip("Origin of the projectile")]
	public Transform arm;


	//Projectile fire restricted by range.
	public void FireProjectile(float distance, float yPos, ref float time)
	{
		//Fire only if within range, and the target isn't too far vertically.
		if (distance < fireRange && yPos <= transform.position.y + vertRange && time >= fireRate ) {

			//Instantiate new bullet.
			GameObject bullet = (GameObject)Instantiate (projectile, arm.position, Quaternion.identity);

			//Fire bullet forward from arm.
			bullet.GetComponent<Rigidbody> ().velocity = arm.forward * projectileSpeed;
			time = 0.0f;
		}
	}

	//Projectile fire ignoring range.
	public void FireProjectile( ref float time)
	{
		if (time >= fireRate ) {

			//Instantiate new bullet.
			GameObject bullet = (GameObject)Instantiate (projectile, arm.position, Quaternion.identity);

			//Fire bullet forward from arm.
			bullet.GetComponent<Rigidbody> ().velocity = arm.forward * projectileSpeed;
			time = 0.0f;
		}
	}
}
