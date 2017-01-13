using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using System.Collections;
using System.Collections.Generic;

public class EventManager : MonoBehaviour {

	//Singleton Pattern. Create a single instance of the Object.
	public static EventManager instance = null;

	[Tooltip("List of Callable Events")]
	public List<LevelEvent> events;

	[Tooltip("Text Box Where Event Text is Shown")]
	public Text dialogueField;

	[Tooltip("Respawn Point for Player Character.")]
	public Transform respawnPoint;

	//Instance of Player in game.
	private PlayerController player;

	void Awake()
	{
		//Instantiate single instance.
		if (!instance)
		{
			instance = this;
		}

		//Get Player.
		player = GameObject.FindGameObjectWithTag ("Player").GetComponent<PlayerController> ();

		//Always start the level with the 1st event.
		TriggerEvent (0);
	}

	//Initiate event with the given ID.
	public void TriggerEvent(int eventID)
	{
		//Initiate Event.
		if (events.Count > eventID) 
		{
			//New thread of event.
			StartCoroutine(ExecuteEvent(events[eventID]));
		} 
		//Event doesn't exist.
		#if UNITY_EDITOR
		else 
		{
			Debug.LogError(string.Format("Missing Event: #{0}, Event Size {1}", eventID, events.Count));
		}
		#endif
	}

	//Event execution, multi-threaded.
	IEnumerator ExecuteEvent(LevelEvent currEvent)
	{
		//Check event exists.
		if (currEvent != null) 
		{
			//Respawn character at Manager's Respawn Point.
			if (currEvent.respawn)
			{
				Respawn ();
			}

			//Disable the Character controller until event finishes.
			if (currEvent.pauseCharacterControl)
			{
				player.enabled = false;
			}

			//Time reference.
			float currTime = 0.0f;

			//Store the text colour to modify the alpha value (for fade-in, fade-out).
			Color color = dialogueField.color;

			//Pre-event delay.
			yield return new WaitForSeconds (currEvent.startDelay);

			//Set the Event's text variables.
			dialogueField.text = currEvent.dialog;
			dialogueField.fontSize = currEvent.fontSize;

			//Colour fade-in. Lerp text's alpha between 0-1.
			if (currEvent.fade) 
			{
				do 
				{
					currTime += Time.deltaTime;
					color.a = Mathf.Lerp (0.0f, 1.0f, currTime);
					dialogueField.color = color;
					yield return null;
				} while(currTime <= currEvent.fadeIn);
			}

			//Mid-event delay.
			yield return new WaitForSeconds (currEvent.duration);

			//Colour fade-out. Lerp text's alpha between 1-0.
			if (currEvent.fade) 
			{
				currTime = 0.0f;
				do {
					currTime += Time.deltaTime;
					color.a = Mathf.Lerp (1.0f, 0.0f, currTime);
					dialogueField.color = color;
					yield return null;
				} while(currTime <= currEvent.fadeOut);
			}

			//Reset text and alpha.
			dialogueField.text = "";
			color.a = 1.0f;
			dialogueField.color = color;

			//Re-enable the player.
			if (currEvent.pauseCharacterControl) 
			{
				player.enabled = true;
			}

			//Load new Scene.
			if (currEvent.loadScene) 
			{
				SceneManager.LoadScene (currEvent.sceneName);
			}
			//Auto-trigger the next event in the list.
			else if (currEvent.autoContinue) 
			{
				StartCoroutine (ExecuteEvent (events [events.IndexOf (currEvent) + 1]));
			} 
		}
	}

	//Reset player position to the Manager's spawn point.
	void Respawn()
	{
		player.gameObject.transform.position = respawnPoint.position;
	}

	//Destroy the singleton instance when the scene exits.
	void OnExit()
	{
		instance = null;
		Destroy (this);
	}
}
