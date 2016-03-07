using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class Exit : MonoBehaviour {
	public GameObject confirmationMenu;
	/// <summary>
	/// Brings up a quit confermation menu
	/// </summary>
	public void quitConfirm(){
		//get the first button in the child of the confirmation menu
		RectTransform quitMenu = Instantiate(confirmationMenu).GetComponent<RectTransform>();
		Button quit = quitMenu.GetChild(1).GetComponent<Button>();
		quit.onClick.AddListener(() => { 
			this.quit(); // call this when confirm button is pressed
		});
		quitMenu.SetParent(GameObject.Find("Canvas").transform); // the name for the Canvas must be this in all Scenes
		quitMenu.anchoredPosition = new Vector2(0,0);
		quitMenu.sizeDelta = new Vector2(0,-250f);
	}

	private void quit(){
		Application.Quit();
	}

	// Use this for initialization
	void Start () {
		DontDestroyOnLoad(transform.gameObject);
		
	}

	// Update is called once per frame
	void Update () {
		if(Input.GetKeyDown(KeyCode.Escape)){
			quitConfirm();
		}
	}
}
