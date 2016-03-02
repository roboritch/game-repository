using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class QuitButton : MonoBehaviour {
	public GameObject confirmationMenu;
	/// <summary>
	/// Brings up a quit confermation menu
	/// </summary>
	public void quitConfirm(){
		Instantiate(confirmationMenu).GetComponent<Button>().onClick.AddListener(() => {
			this.quit(); // call this when button is pressed
		});
	}

	private void quit(){
		Application.Quit();
	}

}
