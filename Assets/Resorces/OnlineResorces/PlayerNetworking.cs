using UnityEngine;
using UnityEngine.Networking;
using System.Collections;

public class PlayerNetworking : NetworkBehaviour{
	NetworkManager network;
	// Use this for initialization
	void Start(){
		GameObject net = GameObject.Find("Network Control");
		if(net == null){
			this.enabled = false;
		}
	}

	public override void OnStartServer(){
		base.OnStartServer();
		Player.Instance.playerAlliance = 1;
	}

	public override void OnStartClient(){
		base.OnStartClient();
		Player.Instance.playerAlliance = 2;
	}

}
