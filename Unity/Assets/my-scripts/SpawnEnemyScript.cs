using UnityEngine;
using System.Collections;
using UnityEngine.Networking;

public class SpawnEnemyScript : NetworkBehaviour {

	public GameObject objectToSpawn;
	public float timeToWaitBetweenSpawns = 2.0f;
	public float timer = 0;

	GameObject[] players;
	bool gameStarted = false;

	void Update () {
		if(!gameStarted)
			return;

		timer += Time.deltaTime;
		if (timer > timeToWaitBetweenSpawns) {
			CmdSpawnEnemy ();
			timer = 0;
		}
	}

	public override void OnStartServer(){
		gameStarted = true;
		timer = 0;
	}

	[Command]
	void CmdSpawnEnemy(){
		GameObject enemy = (GameObject)Instantiate(objectToSpawn, transform.position, transform.rotation);
		players = GameObject.FindGameObjectsWithTag ("Player");
		int index = Random.Range (0, 1000) % players.Length;
		GameObject playerToAttack = players[index];
		enemy.GetComponent<EnemyMovement> ().player = playerToAttack;
		enemy.GetComponent<EnemyAttack> ().player = playerToAttack;
		NetworkServer.Spawn (enemy);
	}
}
