using UnityEngine;
using System.Collections;

public class RightClickMenuExit : MonoBehaviour{

	public void destroy(){
		Destroy(transform.parent.gameObject);
	}

	void Update(){
		if(Input.GetKeyUp(KeyCode.D)){
			destroy();
		}	
	}


}
