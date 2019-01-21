using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using UnityEngine.Networking;

// ready for network multiplayer: NetworkBehaviour {
public class PlayerHealth : NetworkBehaviour {

	public CharacterType characterType;

	// TEMPORARY - NO NETWORK:
	//bool isServer = true;
	//bool isLocalPlayer = true;
	//

	public int startingHealth = 100;

	[SyncVar] public int currentHealth = 100;

	//public int currentHealth = 100;

	float shakingTimer = 0;
	public float timeToShake = 1.0f;
	public float shakeIntensity = 3.0f;
	bool isShaking = false;


	public bool isDead = false;
	Animator anim;

	public Text healthText;

	bool playerIsTakingDamage = false;
	float damageBufferZone = 0.35f; // in seconds

	PlayerAttributes attributes;

	// Use this for initialization
	void Start () {
		currentHealth = startingHealth;
		anim = GetComponent<Animator> ();
		healthText = GameObject.Find ("HealthText").GetComponent<Text>();
		//attributes = GetComponent<PlayerAttributes> ();
		//attributes.InitStatsForCharacterType (characterType);
	}

	void Update () {

		SetHealthText ();

		if (!isDead) {
			if (currentHealth <= 0) {
				currentHealth = 0;
				Death ();
			}
		}

	}
		
	public void TakeDamage(int amount){
		//if (!isServer)
			//return;
		
		if (isDead)
			return;

		// avoid too many damages at once
		if (playerIsTakingDamage)
			return;

		Invoke ("EnableDamage", damageBufferZone);
			
			
		currentHealth -= amount;
		if (currentHealth < 0) {
			currentHealth = 0;
		}
	}

	public void Death(){
		if (isDead)
			return;
		if (isServer) {
			RpcDeath ();
		} else {
			anim.SetTrigger ("Death");
			isDead = true;	
		}
	}

	[ClientRpc]
	public void RpcDeath(){
		anim.SetTrigger ("Death");
		isDead = true;
	}

	void ShakeCamera(){
		shakingTimer = 0;
		isShaking = true;
		
	}
		
	void SetHealthText(){
		if (isLocalPlayer) {
			healthText.text = "HP: " + currentHealth.ToString ();
		}
	}

	void EnableDamage(){
		playerIsTakingDamage = false;
	}

	/*[Command]
	public void CmdTakeDamage(int amount, string playerUniqueName) {
		GameObject p = GameObject.Find (playerUniqueName);
		p.GetComponent<PlayerHealth>().TakeDamage (amount);
	}*/

	[ClientRpc]
	public void RpcTakeDamage(int amount) {
		TakeDamage (amount);
	}

	public void IncreaseNumberOfKills(string playerUniqueName) {
		if (!isDead) {
			GameObject p = GameObject.Find (playerUniqueName);
			p.GetComponent<PlayerKills> ().KillsCount++;
		}
	}

	public void OnTriggerEnter (Collider col)
	{
		GameObject otherObject = col.gameObject;
		if (otherObject.name == "ColliderAttack") {
			GameObject otherObjectPlayer = FindParentWithTag(otherObject, "Player");

			// checking if the ColliderAttacker s from another player
			if (otherObjectPlayer.name != this.gameObject.name) {
				if (otherObjectPlayer.GetComponent<PlayerBehaviorScript> ().isAttacking == false)
					return;


				int damageDealt = otherObjectPlayer.GetComponent<PlayerAttributes> ().computeDamageDealt ();
				int damageReduction = GetComponent<PlayerAttributes> ().computeDamageReduction ();
				int totalDamage = damageDealt - damageReduction;
				if (isServer) {
					TakeDamage (totalDamage);
				} else {
					// find the local player -> tell him/her to apply damage
					GameObject manager = GameObject.FindGameObjectWithTag("Manager");
					ManagerReferences refs = manager.GetComponent<ManagerReferences> ();
					string playerUniqueName = GetComponent<PlayerID> ().playerUniqueName;
					refs.localPlayer.GetComponent<PlayerHealth> ().CmdTakeDamage (totalDamage, playerUniqueName);
				}

			}

		}
	}

	[Command]
	public void CmdTakeDamage(int amount, string uniqueID){
		GameObject p = GameObject.Find (uniqueID);
		p.GetComponent<PlayerHealth> ().TakeDamage (amount);
	}






	public void HandleDamage(int amount, string playerUniqueName) {
		if (isServer) {
			GameObject p = GameObject.Find (playerUniqueName);
			p.GetComponent<PlayerHealth> ().TakeDamage (amount);

		} else {
			GameObject manager = GameObject.FindGameObjectWithTag ("Manager");
			ManagerReferences refs = manager.GetComponent<ManagerReferences> ();
			GameObject localPlayer = refs.localPlayer;
			localPlayer.GetComponent<PlayerHealth> ().CmdTakeDamage (amount, playerUniqueName);
		}
	}

	[Command]
	void CmdTellServerWhoGotShot(string uniqueID, int damage){
		//GameObject obj = GameObject.Find (uniqueID);
		//obj.GetComponent<PlayerHealth> ().TakeDamage (damage);
	}

	public static GameObject FindParentWithTag(GameObject childObject, string tag)
	{
		Transform t = childObject.transform;
		while (t.parent != null)
		{
			if (t.parent.tag == tag)
			{
				return t.parent.gameObject;
			}
			t = t.parent.transform;
		}
		return null; // Could not find a parent with given tag.
	}

}
