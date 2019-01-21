using UnityEngine;
using System.Collections;
using UnityEngine.Networking;

public class ItemGroundScript : NetworkBehaviour {

	public string NameOfItemToEquip;
	public string whereToEquipItem;
	public int quality = 0;
	public string NameOfSlotImage;
	// Use this for initialization
	void Start () {

		SetNameColor (quality);
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}


	void OnTriggerEnter(Collider other){
		if (other.gameObject.tag == "Player") {
			// add item to inventory (backpack)
			GameObject imageObject = Instantiate(Resources.Load(NameOfSlotImage)) as GameObject;
			ItemAttributes attributes = imageObject.AddComponent<ItemAttributes> ();
			ItemAttributes attributesToClone = GetComponent<ItemAttributes> ();
			attributes.cloneAttributesFrom (attributesToClone);

			//  we need to clone the value from the item on the ground to the item image
			GameObject manager = GameObject.FindGameObjectWithTag ("Manager");
			ManagerReferences refs = manager.GetComponent<ManagerReferences> ();

			if (other.GetComponent<PlayerBehaviorScript> ().isLocalPlayer) {
				GameObject backpack = refs.backpack;
				backpack.GetComponent<BackpackManager> ().AddItem (imageObject);

				if (isServer) {
					Destroy (this.gameObject);
				} else {
					refs.localPlayer.GetComponent<PlayerBehaviorScript> ().CmdDestroyObject (this.gameObject);
				}
			}
		}
	}

	public void SetNameColor(int value){
		Color color = Color.gray;
		switch (value) {
		case 1:
			color = Color.white;
			break;
		case 2:
			color = Color.blue;
			break;
		case 3:
			color = Color.yellow;
			break;
		case 4:
			color = new Color (r: 1.0f, g: 155/255, b: 51/255, a: 1.0f); // Orange color
			break;
		default:
			color = Color.black;
			break;
		}

		Transform nameObjectTransform = transform.Find ("Name");
		if(nameObjectTransform == null) {
			Debug.Log (transform.gameObject.name + " has no child object named 'Name'");
			return;
		}
		TextMesh txt = nameObjectTransform.gameObject.GetComponent<TextMesh> ();

		if (txt != null) {
			transform.Find ("Name").gameObject.GetComponent<TextMesh> ().color = color;
		} else {
			Debug.Log ("txt is null");
		}
	}



	[Command]
	void CmdDestroy(GameObject obj)
	{
		NetworkServer.Destroy(obj);
	}
}














