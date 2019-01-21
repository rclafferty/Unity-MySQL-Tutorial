using UnityEngine;
using System.Collections;
using UnityEngine.Networking;

public class EnemyMovement : NetworkBehaviour {

	public GameObject player;
	UnityEngine.AI.NavMeshAgent nav;


	// Use this for initialization
	void Start () {
		nav = GetComponent<UnityEngine.AI.NavMeshAgent> ();
	}
	
	// Update is called once per frame
	void Update () {
		if (!GetComponent<EnemyHealth> ().isDead) {
			if (player.GetComponent<PlayerHealth> ().isDead) {
				GetComponent<EnemyHealth> ().Death ();
				GetComponent<EnemyHealth> ().isDead = true;
			} else {
				nav.SetDestination (player.transform.position);
			}
		}
	}
}
