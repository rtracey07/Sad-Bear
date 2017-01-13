using UnityEngine;
using System.Collections;

/** Detect when player enters Platform trigger, to parent the player to this
 *  platform. Locks player movement relative to the platform while triggered. */
public class PlatformDetector : MonoBehaviour {

	private PlatformController controller;

	void Start()
	{
		//Cache platform controller.
		controller = GetComponentInParent<PlatformController> ();
	}

	void OnTriggerEnter(Collider other)
	{
		//If collision with Player.
		if (other.tag == "Player") 
		{
			//Set the player to be parented by this platform.
			other.transform.SetParent(this.transform);

			//Trigger platform movement, if triggerable.
			if (controller != null) 
			{
				controller.SetTrigger (true);
			}
		}
	}

	void OnTriggerExit(Collider other)
	{
		//Reset the Player's parenting to be root.
		transform.DetachChildren ();

		//Release platform movement trigger, if triggerable.
		if (controller != null) 
		{
			controller.SetTrigger (false);
		}
	}
}
