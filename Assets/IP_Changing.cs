using UnityEngine;
using UnityEngine.Networking;
using System.Collections;
using System.Net;
using System.Net.Sockets;
using System;

public class IP_Changing : NetworkBehaviour{

	public override void OnStartServer(){
		NetworkManager.singleton.serverBindAddress = GetLocalIPAddress();
		base.OnStartServer();
	}

	public static string GetLocalIPAddress(){
		var host = Dns.GetHostEntry(Dns.GetHostName());
		foreach( var ip in host.AddressList ){
			if(ip.AddressFamily == AddressFamily.InterNetwork){
				return ip.ToString();
			}
		}
		throw new Exception("Local IP Address Not Found!");
	}


}
