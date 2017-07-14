using UnityEngine;
using System.Collections;

/// <summary>
/// controls the display of the knife cut animation
/// </summary>
public class knifeCutControler : MonoBehaviour {
	ParticleSystem PS;


	void Awake () {
		PS = GetComponent<ParticleSystem>();
	}

	private int ticks = 0;
	void Update(){
		if(ticks == 0)
			flickerParticals();
		if(++ticks > 2){
			ticks = 0;
		}
	}

	// Use this for initialization
	void Start () {

	}
		
	/// <summary>
	/// Alter trancperincy of particals at random
	/// </summary>
	private void flickerParticals(){
		ParticleSystem.Particle[] particals = new ParticleSystem.Particle[100];
		int pNum;
		pNum = PS.GetParticles(particals);
		for (int i = 0; i < pNum; i++) {
			if(Random.Range(0,4) == 3){ // less likely to be clear
				particals[i].startColor = Color.clear;
			}else{
				particals[i].startColor = Color.white;
			}
		}
		PS.SetParticles(particals,pNum);
	}
}
