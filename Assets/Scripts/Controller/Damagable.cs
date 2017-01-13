using UnityEngine;
using System.Collections;

[RequireComponent(typeof(Collider))]
public class Damagable : MonoBehaviour {

	[Tooltip("Current health of object")]
	public float health;

	[Tooltip("Maximum health value.")]
	public float healthMax;

	public virtual void ChangeHealth(float difference)
	{
		//Update health by difference without exceeding max.
		health = (health + difference > healthMax) ? healthMax : health + difference;

		//Destroy when health is empty.
		if (health < 0) 
		{
			Destroy (this.gameObject);
		}	
	}
}
