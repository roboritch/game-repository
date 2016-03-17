using UnityEngine;
using System.Collections;


public class LongRangeAttackAnimationControler : MonoBehaviour ,IGetAnimationTimeToFin{
	#pragma warning disable // all these must destroy themselves once there animations are done
	[SerializeField] private GameObject pulsePrefab;
	[SerializeField] private GameObject lrFirePrefab; //TODO create fire prefab
	[SerializeField] private GameObject lrHitPrefab; //TODO create hit prefab


	void Start(){
		createPulse();
	}

	#region pulse
	private void createPulse(){
		GameObject temp = Instantiate(pulsePrefab) as GameObject;
		temp.transform.SetParent(transform);
		temp.transform.localPosition = new Vector3();
	}

	private void checkPulseDone(){
		if(transform.childCount == 0){
			fire();
		}
	}
	#endregion

	#region fire
	private void fire(){
		GameObject temp = Instantiate(lrFirePrefab) as GameObject;
		temp.transform.SetParent(transform);
		temp.transform.localPosition = new Vector3();
	}

	private void checkFireDone(){
		if(transform.childCount == 0){
			hit();
		}
	}
	#endregion


	private void hit(){	

	}


	private float foundAnimationTime = 0.0f; //UNDONE this must be found and set
	public float getAnimationTime (){
		return foundAnimationTime;
	}
		

}
