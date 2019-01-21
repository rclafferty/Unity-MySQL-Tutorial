using UnityEngine;
using System.Collections;
using UnityEngine.Networking;

public class LootManager : NetworkBehaviour {

	public GameObject[] lootToSpawn;

	public Vector3 spawnPoint;
	bool gameStarted = false;

	// Use this for initialization
	void Start () {


	
	}
	
	// Update is called once per frame
	void Update () {
		if (gameStarted == true) {
			gameStarted = false;
			CmdSpawnItems ();
		}
	
	}

	public override void OnStartServer ()
	{
		base.OnStartServer ();
		gameStarted = true;

	}

	[Command]
	public void CmdSpawnItems(){
		float offset = 7.0f;
		for (int i = 0; i < lootToSpawn.Length; i++) {
			GameObject loot = GameObject.Instantiate (lootToSpawn [i]);

			loot.transform.position = new Vector3 (spawnPoint.x + offset * ((float)i + 1.0f), spawnPoint.y, spawnPoint.z);
			//loot.transform.rotation = this.gameObject.transform.rotation;

			NetworkServer.Spawn (loot);
		}
	}


}
