using UnityEngine;
using System.Collections;

public class RandomParticleSelect : MonoBehaviour {
	#pragma warning disable
	[SerializeField] private ParticleSystem particleSystem1;
	[SerializeField] private ParticleSystem particleSystem2;
	[SerializeField] private float emitionRateInSecond;

	void Start(){
		InvokeRepeating("emitRandomParticle",0,emitionRateInSecond);
	
	}


	private void emitRandomParticle(){
		if(Random.Range(0,2) == 0){

		}else{

		}
	}





}
