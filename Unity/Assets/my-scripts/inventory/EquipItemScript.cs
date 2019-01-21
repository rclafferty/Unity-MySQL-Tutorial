using UnityEngine;
using System.Collections;
using UnityEngine.Networking;

public class EquipItemScript : NetworkBehaviour {

	public GameObject HookRightHand;
	public GameObject HookLeftForeArm;
	public GameObject HookHead;
	public GameObject HookLeftShoulder;
	public GameObject HookRightShoulder;
	public GameObject HookBuckle;

	public int useEquipmentSet = 0;

	public ItemType currentWeaponType;

	void Start(){
	}

	public override void OnStartLocalPlayer (){
		base.OnStartLocalPlayer ();
	}

	void computeScale(GameObject obj) {
		float scale = GameObject.FindGameObjectWithTag ("Manager").GetComponent<ManagerReferences> ().playerScale;
		Vector3 sc = obj.transform.localScale;
		obj.transform.localScale = new Vector3(sc.x * scale, sc.y * scale, sc.z * scale);
	}

	public void SetItemAttributes(GameObject itemToEquip, string targetPlayerName, Attributes attributes){
		if (!isLocalPlayer)
			return;
		if (gameObject.name != targetPlayerName)
			return;

		ItemAttributes itemToEquipAttributes = itemToEquip.AddComponent<ItemAttributes> ();
		itemToEquipAttributes.cloneAttributesFrom (attributes);
		GetComponent<PlayerAttributes>().ComputeStats();
	}

	public ManagerReferences getManagerRefs() {
		GameObject manager = GameObject.FindGameObjectWithTag ("Manager");
		return manager.GetComponent<ManagerReferences> ();
	}


	public void HookItem (string itemPath, string targetPlayerName, Attributes attributes, GameObject hook) {
		for (int i = 0; i < hook.transform.childCount; i++) {
			Destroy (hook.transform.GetChild (i).gameObject);
		}
		GameObject item = Instantiate( Resources.Load(itemPath)) as GameObject;

		item.transform.parent = hook.transform;
		Vector3 offset = item.GetComponent<HookScript> ().positionOffset;
		item.transform.position = hook.transform.position;
		item.transform.rotation = hook.transform.rotation;
		item.transform.localPosition = Vector3.zero + offset;
		Vector3 rotOffset = item.GetComponent<HookScript> ().rotationOffset;
		item.transform.Rotate (rotOffset);
		computeScale (item);

		SetItemAttributes (item, targetPlayerName, attributes);
	}

	[Command]
	public void CmdEquipItemOnRightHand(string itemPath, string targetPlayerName, Attributes attributes) {
		RpcEquipItemOnRightHand (itemPath, targetPlayerName, attributes);
	}

	[ClientRpc]
	public void RpcEquipItemOnRightHand(string itemPath, string targetPlayerName, Attributes attributes) {
		if (targetPlayerName != gameObject.name) {
			return;
		}
		HookItem (itemPath, targetPlayerName, attributes, HookRightHand);
	}

	[Command]
	public void CmdEquipItemOnLeftHand(string itemPath, string targetPlayerName, Attributes attributes) {
		RpcEquipItemOnLeftHand (itemPath, targetPlayerName, attributes);
	}

	[ClientRpc]
	public void RpcEquipItemOnLeftHand(string itemPath, string targetPlayerName, Attributes attributes) {
		if (targetPlayerName != gameObject.name) {
			return;
		}
		HookItem (itemPath, targetPlayerName, attributes, HookLeftForeArm);
	}

	[Command]
	public void CmdEquipItemOnHead(string itemPath, string targetPlayerName, Attributes attributes) {
		RpcEquipItemOnHead (itemPath, targetPlayerName, attributes);
	}

	[ClientRpc]
	public void RpcEquipItemOnHead(string itemPath, string targetPlayerName, Attributes attributes) {
		if (targetPlayerName != gameObject.name)
			return;
		HookItem (itemPath, targetPlayerName, attributes, HookHead);
	}

	[Command]
	public void CmdEquipLeftShoulderWith(string itemPath, string targetPlayerName, Attributes attributes) {
		RpcEquipItemOnLeftShoulder (itemPath, targetPlayerName, attributes);
	}

	[ClientRpc]
	public void RpcEquipItemOnLeftShoulder(string itemPath, string targetPlayerName, Attributes attributes) {
		if (targetPlayerName != gameObject.name)
			return;
		HookItem (itemPath, targetPlayerName, attributes, HookLeftShoulder);
	}

	[Command]
	public void CmdEquipRightShoulderWith(string itemPath, string targetPlayerName, Attributes attributes) {
		RpcEquipItemOnRightShoulder (itemPath, targetPlayerName, attributes);
	}

	[ClientRpc]
	public void RpcEquipItemOnRightShoulder(string itemPath, string targetPlayerName, Attributes attributes) {
		if (targetPlayerName != gameObject.name)
			return;
		HookItem (itemPath, targetPlayerName, attributes, HookRightShoulder);
	}

	[Command]
	public void CmdEquipBuckleWith(string itemPath, string targetPlayerName, Attributes attributes) {
		RpcEquipItemOnBuckle (itemPath, targetPlayerName, attributes);
	}

	[ClientRpc]
	public void RpcEquipItemOnBuckle(string itemPath, string targetPlayerName, Attributes attributes) {
		if (targetPlayerName != gameObject.name)
			return;
		HookItem (itemPath, targetPlayerName, attributes, HookBuckle);
	}
		
	void EquipTestItems1(){
		GameObject manager = GameObject.FindGameObjectWithTag ("Manager");
		ManagerReferences refs = manager.GetComponent<ManagerReferences> ();
		Attributes attributes = new Attributes ();
		attributes.Fill (5, 1, 4, 5);

		CmdEquipItemOnRightHand ("MyPrefabs/gear/sword-simple-1", refs.localPlayer.name, attributes);
		CmdEquipItemOnLeftHand ("MyPrefabs/gear/shield-3", refs.localPlayer.name, attributes);
		CmdEquipItemOnHead("MyPrefabs/gear/helmet-3", refs.localPlayer.name, attributes);
		CmdEquipLeftShoulderWith("MyPrefabs/gear/shoulder-1-left", refs.localPlayer.name, attributes);
		CmdEquipRightShoulderWith("MyPrefabs/gear/shoulder-1-right", refs.localPlayer.name, attributes);
		CmdEquipBuckleWith("MyPrefabs/gear/buckle-1", refs.localPlayer.name, attributes);

	}

	void EquipTestItems2(){
		GameObject manager = GameObject.FindGameObjectWithTag ("Manager");
		ManagerReferences refs = manager.GetComponent<ManagerReferences> ();
		Attributes attributes = new Attributes ();
		attributes.Fill (5, 1, 4, 5);

		CmdEquipItemOnRightHand ("MyPrefabs/gear/sword-simple-2", refs.localPlayer.name, attributes);
		CmdEquipItemOnLeftHand ("MyPrefabs/gear/shield-round-wood", refs.localPlayer.name, attributes);
		CmdEquipItemOnHead("MyPrefabs/gear/helmet-2", refs.localPlayer.name, attributes);
		CmdEquipLeftShoulderWith("MyPrefabs/gear/shoulder-2-left", refs.localPlayer.name, attributes);
		CmdEquipRightShoulderWith("MyPrefabs/gear/shoulder-2-right", refs.localPlayer.name, attributes);
		CmdEquipBuckleWith("MyPrefabs/gear/buckle-2", refs.localPlayer.name, attributes);
	}

	[Command]
	public void CmdUnequipItem(ItemType itemType, string targetPlayerName) {
		RpcUnequipItem (itemType, targetPlayerName);
	}

	[ClientRpc]
	public void RpcUnequipItem(ItemType itemType, string targetPlayerName) {
		if (targetPlayerName != gameObject.name)
			return;
		GameObject item = null;
		GameObject item2 = null; // i.e 2 shoulder pads

		switch (itemType) {
		case ItemType.Helmet:
			item = HookHead;
			break;

		case ItemType.Shoulders:
			item = HookLeftShoulder;
			item2 = HookRightShoulder;
			break;

		case ItemType.Weapon:
			item = HookRightHand;
			break;

		case ItemType.WeaponArchery:
			item = HookRightHand;
			break;

		case ItemType.Shield:
			item = HookLeftForeArm;
			break;

		case ItemType.Buckle:
			item = HookBuckle;
			break;

		default:
			break;
		}

		if (item != null) {
			for (int i = 0; i < item.transform.childCount; i++) {
				DestroyImmediate (item.transform.GetChild (i).gameObject);
			}
		}
		if (item2 != null) {
			for (int i = 0; i < item2.transform.childCount; i++) {
				DestroyImmediate (item2.transform.GetChild (i).gameObject);
			}
		}

		GetComponent<PlayerAttributes>().ComputeStats();

	}

	public void UnequipHead(){
		CmdUnequipItem(ItemType.Helmet, getManagerRefs().localPlayer.name);
	}

	public void UnequipShoulders(){
		CmdUnequipItem(ItemType.Shoulders, getManagerRefs().localPlayer.name);
	}

	public void UnequipRightHand(){
		CmdUnequipItem(ItemType.Weapon, getManagerRefs().localPlayer.name);
	}

	public void UnequipShield(){
		CmdUnequipItem(ItemType.Shield, getManagerRefs().localPlayer.name);
	}

	public void UnequipBuckle(){
		CmdUnequipItem(ItemType.Buckle, getManagerRefs().localPlayer.name);
	}
		

}
















