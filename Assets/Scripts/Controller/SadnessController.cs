using UnityEngine;
using UnityEngine.UI;
using System.Collections;

[RequireComponent(typeof(Collider))]
[ExecuteInEditMode]
public class SadnessController : Damagable {

	[Tooltip("Minimum scale value of GameObject")]
	public int minSize;

	[Tooltip("Maximum scale value of GameObject")]
	public int maxSize;

	[Tooltip("UI Slider where Sadness level will be displayed")]
	public Slider sadnessMeter;

	void Update () {
		Scale ();
	}

	void Scale()
	{
		//Update scale of object to match health %.
		float scaleSize = Mathf.Lerp (minSize, maxSize, health/100);
		transform.localScale = new Vector3 (scaleSize, scaleSize, scaleSize);

		//Update UI.
		if (sadnessMeter) 
		{
			sadnessMeter.value = health;
		}
	}
		
	//Run Coroutine to shrink/grow object size.
	override public void ChangeHealth(float difference)
	{
		StartCoroutine (FadeHealth (1, health+difference, health, difference));
	}

	//Couroutine for shrink/grow object size.
	IEnumerator FadeHealth(float time, float newHealth, float currHealth, float difference)
	{
		float currTime = 0.0f;

		//Lerp between given sizes over time.
		do {
			health = Mathf.Lerp(currHealth, newHealth, currTime);
			currTime += Time.deltaTime;
			yield return null;
		} 
		while(currTime <= time);

		//Check if dead.
		if (health <= 0) 
		{
			health = 0;
			EventManager.instance.TriggerEvent (6);
			health = 50;
		} 
		//Check if full sadness (win state).
		else if (health >= 100) 
		{
			health = 100;
			EventManager.instance.TriggerEvent (5);
		}
	}

	//Get enemy collision.
	void OnTriggerEnter(Collider other)
	{
		if (other.tag == "Enemy") 
		{
			Enemy enemy = other.GetComponent<Enemy> ();

			//Sad enemies increase your sadness on collision. Absorb them.
			if (enemy && enemy.isSad) 
			{
				ChangeHealth (enemy.health);
				Destroy (enemy.gameObject);
			} 
			//Happy enemies deal damage equal to their health.
			else 
			{
				ChangeHealth (-enemy.health);
			}
		}
	}
}
