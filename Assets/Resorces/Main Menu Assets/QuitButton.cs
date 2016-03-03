using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class QuitButton : MonoBehaviour {
	public GameObject confirmationMenu;
	/// <summary>
	/// Brings up a quit confermation menu
	/// </summary>
	public void quitConfirm(){
		Button quit = Instantiate(confirmationMenu).GetComponent<Button>();
		quit.onClick.AddListener(() => { 
			this.quit(); // call this when confirm button is pressed
		});

		RectTransform trans = quit.GetComponent<RectTransform>();
		trans.SetParent(transform.parent); // perent to canvis
		trans.localPosition = new Vector3(); //display in the center
		trans.anchoredPosition = new Vector2();

	}

	private void quit(){
		Application.Quit();
	}

}
