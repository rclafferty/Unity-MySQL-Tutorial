﻿using UnityEngine;
using System.Collections;
using UnityEngine.Networking;
using UnityEngine.UI;

public class PlayerKills : NetworkBehaviour {

	[SyncVar]
	public int KillsCount = 0;

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
		SetKillsCountText ();
	}

	public void SetKillsCountText(){
		if (isLocalPlayer) {
			GameObject killsText = GameObject.Find ("KillsText");
			killsText.GetComponent<Text> ().text = "Kills: " + KillsCount.ToString ();
		}
	}
}

















