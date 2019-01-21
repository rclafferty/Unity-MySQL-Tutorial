using UnityEngine;
using System.Collections;

public class SlotEquipItem : MonoBehaviour {

	public GameObject player;
	public string pathOfItemToEquip;
	public string pathOfItemToEquip2;

	void Start(){
	}

	public Attributes GetAttributesFromItemImage() {
		Attributes newAttributes = new Attributes ();
		newAttributes.cloneAttributesFrom (DragDropScript.draggedItem.GetComponent<ItemAttributes> ());

		return newAttributes;
	}

	public void EquipItemOnHead(){
		getLocalPlayer ();
		string itemPath = DragDropScript.draggedItem.GetComponent<DragDropScript>().pathOfItemToEquip;
		player.GetComponent<EquipItemScript> ().CmdEquipItemOnHead (itemPath, player.name, GetAttributesFromItemImage() );
	}

	public void EquipItemOnShoulders(){
		getLocalPlayer ();

		string itemPath = DragDropScript.draggedItem.GetComponent<DragDropScript>().pathOfItemToEquip;
		player.GetComponent<EquipItemScript> ().CmdEquipLeftShoulderWith (itemPath, player.name, GetAttributesFromItemImage() );

		string itemPath2 = DragDropScript.draggedItem.GetComponent<DragDropScript>().pathOfItemToEquip2;
		player.GetComponent<EquipItemScript> ().CmdEquipRightShoulderWith (itemPath, player.name, GetAttributesFromItemImage() );
	}

	public void EquipWeapon(ItemType itemType){
		getLocalPlayer ();
		string itemPath = DragDropScript.draggedItem.GetComponent<DragDropScript>().pathOfItemToEquip;
		player.GetComponent<EquipItemScript> ().CmdEquipItemOnRightHand (itemPath, player.name, GetAttributesFromItemImage() );
		player.GetComponent<EquipItemScript> ().currentWeaponType = itemType;

	}

	public void EquipShield(){
		getLocalPlayer ();
		string itemPath = DragDropScript.draggedItem.GetComponent<DragDropScript>().pathOfItemToEquip;
		player.GetComponent<EquipItemScript> ().CmdEquipItemOnLeftHand (itemPath, player.name, GetAttributesFromItemImage() );
	}

	public void EquipBuckle(){
		getLocalPlayer ();
		string itemPath = DragDropScript.draggedItem.GetComponent<DragDropScript>().pathOfItemToEquip;
		player.GetComponent<EquipItemScript> ().CmdEquipBuckleWith (itemPath, player.name, GetAttributesFromItemImage() );
	}

	public void UnequipHelmet(){
		getLocalPlayer ();
		player.GetComponent<EquipItemScript> ().UnequipHead ();
		// todo: create a Remove attribute
		player.GetComponent<PlayerAttributes>().ComputeStats();
	}

	public void UnequipShoulders(){
		getLocalPlayer ();
		player.GetComponent<EquipItemScript> ().UnequipShoulders ();
		player.GetComponent<PlayerAttributes>().ComputeStats();
	}

	public void UnequipWeapon(){
		getLocalPlayer ();
		player.GetComponent<EquipItemScript> ().UnequipRightHand ();
		player.GetComponent<PlayerAttributes>().ComputeStats();
	}

	public void UnequipShield(){
		getLocalPlayer ();
		player.GetComponent<EquipItemScript> ().UnequipShield ();
		player.GetComponent<PlayerAttributes>().ComputeStats();
	}

	public void UnequipBuckle(){
		getLocalPlayer ();
		player.GetComponent<EquipItemScript> ().UnequipBuckle ();
		player.GetComponent<PlayerAttributes>().ComputeStats();
	}


	public void SetItemAttributes(GameObject itemToEquip){
		GameObject itemImage = DragDropScript.draggedItem;
		ItemAttributes attributes = itemImage.GetComponent<ItemAttributes> ();
		ItemAttributes itemToEquipAttributes = itemToEquip.AddComponent<ItemAttributes> ();
		itemToEquipAttributes.cloneAttributesFrom (attributes);
		// todo: update the User Interface to display the new attributes
		getLocalPlayer ();
		player.GetComponent<PlayerAttributes>().ComputeStats();

	}

	public void getLocalPlayer() {
		if (player == null) {
			GameObject manager = GameObject.FindGameObjectWithTag ("Manager");
			ManagerReferences refs = manager.GetComponent<ManagerReferences> ();
			player = refs.localPlayer;
			//Debug.Log("just set local player name to " + player.name);
		} else {
			//Debug.Log("local player name is " + player.name);
		}
	}

}









