using UnityEngine;
using System.Collections;
using UnityEngine.EventSystems;
using UnityEngine.Networking;

public class DragDropScript : NetworkBehaviour, IBeginDragHandler, IDragHandler, IEndDragHandler, IPointerEnterHandler, IPointerExitHandler {

	public static GameObject draggedItem;
	public ItemType itemType;
	public string pathOfItemToEquip;
	public string pathOfItemToEquip2;

	public static GameObject statsItemUI;
	public float statsItemUIOffsetY = 60;
	public float statsItemUIOffsetX = 60;


	Vector3 startPosition;
	Transform startParent; // this is the original slot the item was inside of

	public override void OnStartLocalPlayer(){
		GameObject manager = GameObject.FindGameObjectWithTag ("Manager");
		ManagerReferences refs = manager.GetComponent<ManagerReferences> ();
		statsItemUI = refs.StatsItemUI;
		statsItemUI.SetActive (false); // it is now hidden;
	}

	public void Start(){
		//statsItemUI = GameObject.Find ("PanelItemStats") as GameObject;
	}

	public void OnBeginDrag(PointerEventData eventData){
		statsItemUI.SetActive (false);
		draggedItem = gameObject;
		startPosition = transform.position;
		startParent = transform.parent;
		GetComponent<CanvasGroup>().blocksRaycasts = false;
	}

	public void OnDrag (PointerEventData eventData){
		transform.position = eventData.position;
	}

	public void OnEndDrag(PointerEventData eventData){
		draggedItem = null;
		GetComponent<CanvasGroup>().blocksRaycasts = true;
		if (transform.parent == startParent) {
			transform.position = startPosition;
		}
	}

	public void OnPointerEnter(PointerEventData eventData){
		if (statsItemUI == null) {
			GameObject manager = GameObject.FindGameObjectWithTag ("Manager");
			ManagerReferences refs = manager.GetComponent<ManagerReferences> ();
			statsItemUI = refs.StatsItemUI;
		}


		statsItemUI.SetActive (true);
		Vector3 pos = transform.position;
		float computedOffsetX = statsItemUIOffsetX;
		if (pos.x > Screen.width / 2) {
			computedOffsetX = computedOffsetX * -2.5f;	
		}
		statsItemUI.transform.position = new Vector3(pos.x + computedOffsetX, pos.y + statsItemUIOffsetY, pos.z );

		PanelItemStatsScript statsScript = statsItemUI.GetComponent<PanelItemStatsScript> ();
		//draggedItem = gameObject;
		ItemAttributes attributes = gameObject.GetComponent<ItemAttributes> ();

		if (attributes == null) {
			Debug.Log("ItemAttributes is null");
		}

		if (statsScript == null) {
			Debug.Log("statsScript is null");
		}

		statsScript.textStrength.text = "Strength: " + attributes.Strength.ToString();
		statsScript.textDexterity.text = "Dexterity: " + attributes.Dexterity.ToString();
		statsScript.textIntelligence.text = "Intelligence: " + attributes.Intelligence.ToString();
		statsScript.textDamage.text = "Damage: " + attributes.Damage.ToString();


	}

	public void OnPointerExit(PointerEventData eventData){
		statsItemUI.SetActive (false);
	}

}












