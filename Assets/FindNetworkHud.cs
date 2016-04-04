using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Networking;
using System.Collections;

public class FindNetworkHud : MonoBehaviour{
	
	// Use this for initialization
	void Start(){
		NetworkManagerHUD h = GameObject.Find("Network Control").GetComponent<NetworkManagerHUD>();
		UnityEngine.Events.UnityAction<bool> action = new UnityEngine.Events.UnityAction<bool>((bool arg0) => h.enabled = !h.enabled);
		GetComponent<Toggle>().onValueChanged.AddListener(action);
		NetworkDiscovery d = GameObject.Find("Network Control").GetComponent<NetworkDiscovery>();
		action = new UnityEngine.Events.UnityAction<bool>((bool arg0) => d.enabled = !d.enabled);
		GetComponent<Toggle>().onValueChanged.AddListener(action);


	}

}
