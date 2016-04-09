using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class AnimationHolder : Singleton<AnimationHolder>{
	protected AnimationHolder(){
	}
	#pragma warning disable
	[SerializeField] private Animations[] animationLibrarySetter;
	private Dictionary<string,GameObject> animationLibrary = new Dictionary<string, GameObject>(10);

	private void animationLibrarySetup(){
		for(int i = 0; i < animationLibrarySetter.Length; i++){
			animationLibrary.Add(animationLibrarySetter[i].animationName, animationLibrarySetter[i].animation);
		}
	}

	void Start(){
		animationLibrarySetup();
	}

	/// <summary>
	/// Get an un-instateated instance of a pertiular unit perfab
	/// </summary>
	/// <returns>The unit from name.</returns>
	/// <param name="name">Name.</param>
	public GameObject getAnimationFromName(string name){ 
		GameObject un;
		animationLibrary.TryGetValue(name, out un);
		return un;
	}


}



