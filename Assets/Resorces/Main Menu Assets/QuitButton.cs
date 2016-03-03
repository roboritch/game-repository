using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class QuitButton : MonoBehaviour {
	public GameObject confirmationMenu;
	/// <summary>
	/// Brings up a quit confermation menu
	/// </summary>
	public void quitConfirm(){
		//get the first button in the child of the confirmation menu
		RectTransform quitMenu = Instantiate(confirmationMenu).GetComponent<RectTransform>();
		Button quit = quitMenu.GetChild(0).GetComponent<Button>();
		quit.onClick.AddListener(() => { 
			this.quit(); // call this when confirm button is pressed
		});
		quitMenu.SetParent(transform.parent); // tha canvas

	}

	private void quit(){
		Application.Quit();
	}

}
