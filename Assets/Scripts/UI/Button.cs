using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using UnityEngine.EventSystems;
using System.Collections;
using System.Collections.Generic;

/** Controls for UI buttons, or objects acting as clickable. **/
public class Button : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler {

	//List of events triggered by button.
	public List<AnimatorEvent> animators;

	//On mouse over UI button, set animator states.
	public void OnPointerEnter(PointerEventData data)
	{
		//Set each animator state.
		foreach(AnimatorEvent e in animators)
		{
			if (!string.IsNullOrEmpty(e.enterStateName)) 
			{
				e.animator.SetBool (e.enterStateName, e.enterStateValue);
			}
		}
	}

	//On mouse over Object acting as button, set animator states.
	public void OnMouseEnter()
	{
		//Set each animator state.
		foreach(AnimatorEvent e in animators)
		{
			if (!string.IsNullOrEmpty(e.enterStateName)) 
			{
				e.animator.SetBool (e.enterStateName, e.enterStateValue);
			}
		}
	}

	//On mouse leaving UI button, set animator states.
	public void OnPointerExit(PointerEventData data)
	{
		//Set each animator state.
		foreach(AnimatorEvent e in animators)
		{
			if (!string.IsNullOrEmpty(e.exitStateName)) 
			{
				e.animator.SetBool (e.exitStateName, e.exitStateValue);
			}
		}
	}

	//On mouse leaving Object acting as button, set animator states.
	public void OnMouseExit()
	{
		//Set each animator state.
		foreach(AnimatorEvent e in animators)
		{
			if (!string.IsNullOrEmpty(e.exitStateName)) 
			{
				e.animator.SetBool (e.exitStateName, e.exitStateValue);
			}
		}
	}

	//On click of UI button.
	public void OnSelect()
	{
		//Set each animator state.
		foreach(AnimatorEvent e in animators)
		{
			if (!string.IsNullOrEmpty(e.selectStateName)) 
			{
				e.animator.SetBool (e.selectStateName, e.selectStateValue);
			}

			//Load scene if selected.
			if (e.loadScene) 
			{
				SceneManager.LoadScene (e.sceneName);
			}

			//Quit game if selected.
			if (e.quit) 
			{
				Application.Quit ();
			}
		}
	}
		
	//On click for objects acting as buttons.
	public void OnMouseDown()
	{
		//Set each animator state.
		foreach(AnimatorEvent e in animators)
		{
			if (!string.IsNullOrEmpty(e.selectStateName)) 
			{
				e.animator.SetBool (e.selectStateName, e.selectStateValue);
			}

			//Load scene if selected.
			if (e.loadScene) 
			{
				SceneManager.LoadScene (e.sceneName);
			}

			//Quit game if selected.
			if (e.quit) 
			{
				Application.Quit ();
			}
		}
	}
}
