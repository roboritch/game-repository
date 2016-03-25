using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class Exit : MonoBehaviour{

	public static Exit Instance;
	public GameObject confirmationMenu;

	/// <summary>
	/// Awake this instance.
	/// </summary>
	void Awake(){
		if(Instance){
			DestroyImmediate(gameObject);
		} else{
			DontDestroyOnLoad(gameObject);
			Instance = this;
		}
	}

	/// <summary>
	/// Brings up a quit confirmation menu.
	/// </summary>
	public void quitConfirm(){
		//Get the first button in the child of the confirmation menu.
		RectTransform quitMenu = Instantiate(confirmationMenu).GetComponent<RectTransform>();
		Button quit = quitMenu.GetChild(1).GetComponent<Button>();
		quit.onClick.AddListener(() =>{ 
			// Call this when confirm button is pressed.
			this.quit();
		});
		// The name for the Canvas must be this in all scenes.
		quitMenu.SetParent(GameObject.Find("Canvas").transform);
		quitMenu.anchoredPosition = new Vector2(0, 0);
		quitMenu.sizeDelta = new Vector2(0, -250f);
	}

	/// <summary>
	/// Quit this instance.
	/// </summary>
	private void quit(){
		Application.Quit();
	}

	// Update is called once per frame.
	/// <summary>
	/// Update this instance.
	/// </summary>
	void Update(){
		if(Input.GetKeyDown(KeyCode.Escape)){
			quitConfirm();
		}
	}
}
