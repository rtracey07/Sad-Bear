using UnityEngine;
using System.Collections;

public class Exit : MonoBehaviour {

	[Tooltip("Index # of event to play on successful exit.")]
	public int exitEventNumber;

	[Tooltip("Index # of event to play on failed exit.")]
	public int stayEventNumber;

	//Cached Variables.
	private SadnessController controller;

	void OnTriggerEnter(Collider other)
	{
		//Event is triggered on collision with the player's barrier.
		if (other.tag == "Barrier") 
		{
			controller = other.GetComponent<SadnessController> ();

			//Check if at max sadness (win state).
			if (controller.health >= controller.healthMax) 
			{
				EventManager.instance.TriggerEvent (exitEventNumber);
			} 
			//Play event to inform player they cannot exit yet.
			else 
			{
				EventManager.instance.TriggerEvent (stayEventNumber);
			}
		}
	}
}
