using UnityEngine;
using System.Collections;
using UnityEngine.Networking;

public class VehicleCollision : NetworkBehaviour {

	public AudioSource audioSource;
	public AudioClip clip;
	// Use this for initialization
	void Start () {
		audioSource = GetComponent<AudioSource> ();
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}

	void OnTriggerEnter(Collider other){
		if (other.gameObject.tag == "Player") {
			PlayerHealth playerHealth = other.gameObject.GetComponent<PlayerHealth>();
			//playerHealth.Death ();
			playerHealth.TakeDamage(playerHealth.startingHealth);
			CmdTellServerWhoGotShot (other.gameObject.name, playerHealth.startingHealth);
			audioSource.PlayOneShot (clip);
		} else if (other.gameObject.tag == "Enemy") {
			EnemyHealth enemyHealth = other.gameObject.GetComponent<EnemyHealth> ();
			enemyHealth.Death ();
			audioSource.PlayOneShot (clip);

		}
	}

	[Command]
	void CmdTellServerWhoGotShot(string uniqueID, int damage){
		GameObject obj = GameObject.Find (uniqueID);
		obj.GetComponent<PlayerHealth> ().TakeDamage (damage);
	}
}
