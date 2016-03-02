using UnityEngine;
using System.Collections;
using UnityEngine.SceneManagement;

public class LoadOnClick : MonoBehaviour {

	public string sceneName;

	public void LoadScene() {
		SceneManager.LoadScene("stage 1");
	}

}
