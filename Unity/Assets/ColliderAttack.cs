using UnityEngine;
using System.Collections;
using UnityEngine.Networking;

public class ColliderAttack : NetworkBehaviour
{

	public GameObject owner;


	public ManagerReferences getManagerRefs ()
	{
		GameObject manager = GameObject.FindGameObjectWithTag ("Manager");
		return manager.GetComponent<ManagerReferences> ();
	}

	// Use this for initialization
	void Start ()
	{
		if (owner == null) {
			ManagerReferences refs = getManagerRefs ();
			//owner = FindParentWithTag (this.gameObject, "Player"); //refs.localPlayer;
		}
		
	}
	
	// Update is called once per frame
	void Update ()
	{
	
	}

	public void OnTriggerEnter (Collider col)
	{
		GameObject otherObject = col.gameObject;
		if (otherObject.name == "ColliderDefense") {
			
			Debug.Log ("attack got blocked by a shield");
			// todo: find out if the player is in blocking state (animation)
			// and if the player is blocking, then take less damage

		} else if (otherObject.name == "ColliderAttack") {
			


			Debug.Log ("attack got parried");


		} else if (otherObject.tag == "Player") {
			/*PlayerHealth playerHealth = col.gameObject.GetComponent<PlayerHealth> ();
			if (playerHealth != null && playerHealth.currentHealth > 0) {
				GameObject player = playerHealth.gameObject;

				owner = FindParentWithTag (this.gameObject, "Player");
				if (owner == null) {
					Debug.Log ("could not find player parent node");
				}
				if (player.GetComponent<PlayerID> ().playerUniqueName != owner.GetComponent<PlayerID> ().playerUniqueName) {
					if (owner.GetComponent<PlayerBehaviorScript> ().isAttacking == false)
						return;
					
					CmdTellServerWhoGotShot (player.GetComponent<PlayerID> ().playerUniqueName, 20);
					if (playerHealth.currentHealth <= 0) {
						IncreaseNumberOfKillsForPlayerWithName(owner.GetComponent<PlayerID> ().playerUniqueName);
					}
				}
			}

			// we are hitting someone else (not ourself)
			PlayerID ownerPlayerID = owner.GetComponent<PlayerID> ();
			PlayerID otherPlayerID = otherObject.GetComponent<PlayerID> ();

			if (otherObject.name == "Dummy") {
				
				// todo: deal damage on dummy
				Debug.Log("other is dummy");

			} 
			else if (ownerPlayerID != null && otherPlayerID != null) {
				if (ownerPlayerID.playerUniqueName != otherPlayerID.playerUniqueName) {
					if(owner.GetComponent<PlayerBehaviorScript>().isAttacking) {
						GameObject obj = GameObject.Find (otherPlayerID.playerUniqueName);
						int damageDealt = owner.GetComponent<PlayerAttributes> ().computeDamageDealt ();
						int damageReduction = obj.GetComponent<PlayerAttributes> ().computeDamageReduction ();
						int totalDamage = damageDealt - damageReduction;
						obj.GetComponent<PlayerHealth> ().HandleDamage(totalDamage);
					}
				}
			}*/

		}

	}

	[Command]
	void CmdTellServerWhoGotShot(string uniqueID, int damage){
		Debug.Log ("CmdTellServerWhoGotShot");
		GameObject obj = GameObject.Find (uniqueID);
		obj.GetComponent<PlayerHealth> ().TakeDamage (damage);
	}

	void IncreaseNumberOfKillsForPlayerWithName(string playerUniqueName){
		GameObject obj = GameObject.Find (playerUniqueName);
		obj.GetComponent<PlayerKills>().KillsCount++;	
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












