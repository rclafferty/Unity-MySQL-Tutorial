using UnityEngine;
using System.Collections;
using UnityEngine.Networking;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class PlayerRespawn : NetworkBehaviour {

	public GameObject localPlayer;
	public GameObject buttonRespawn;
	private PlayerHealth healthScript;

	bool respawning = false;
	public int countDownStartingValue = 9;
	private int countDownCurrentValue;

	// Use this for initialization
	void Start () {
		respawning = false;
	}
	
	void Update () {
		if (GetComponent<PlayerHealth> ().currentHealth <= 0 && !respawning) {
			respawning = true;
				Invoke ("RpcRespawn", countDownStartingValue);
			if (isLocalPlayer) { 
				GameObject TextRespawnObject = GameObject.Find ("TextRespawn");
				Text TextRespawn = TextRespawnObject.GetComponent<Text> ();
				countDownCurrentValue = countDownStartingValue;
				InvokeRepeating ("UpdateRespawnText", 1.0f, 1.0f);
				TextRespawn.text = countDownCurrentValue.ToString();
			}
		}
	}

	public override void OnStartLocalPlayer(){
		healthScript = GetComponent<PlayerHealth> ();
	}

	[ClientRpc]
	public void RpcRespawn()
	{
		Transform spawn = NetworkManager.singleton.GetStartPosition();
		transform.position = spawn.position;
		GetComponent<PlayerHealth> ().currentHealth = GetComponent<PlayerHealth> ().startingHealth;
		GetComponent<Animator> ().Play ("idlewalk");
		GetComponent<PlayerHealth> ().isDead = false;
		//GetComponent<PlayerShoot> ().isShooting = false;
		//GetComponent<PlayerShoot> ().isEnabled = true;
		respawning = false;
	}

	void UpdateRespawnText(){
		GameObject TextRespawnObject = GameObject.Find ("TextRespawn");
		Text TextRespawn = TextRespawnObject.GetComponent<Text> ();
		countDownCurrentValue--;
		TextRespawn.text = countDownCurrentValue.ToString();
		if (countDownCurrentValue <= 0) {
			CancelInvoke ("UpdateRespawnText");
			TextRespawn.text = "";
		}
	}

}
