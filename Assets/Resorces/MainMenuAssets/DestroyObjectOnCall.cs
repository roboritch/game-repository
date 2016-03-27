using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class DestroyObjectOnCall : MonoBehaviour{

	public void destroy(){
		Destroy(transform.parent.gameObject);
	}

	void Update(){
		if(Input.GetKey(KeyCode.Y)){
			GetComponent<Button>().onClick.Invoke();
		}
		if(Input.GetKey(KeyCode.N)){
			destroy();
		}
	}

}
