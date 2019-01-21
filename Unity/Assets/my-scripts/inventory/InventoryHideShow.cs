using UnityEngine;
using System.Collections;

public class InventoryHideShow : MonoBehaviour {

	public GameObject inventoryUI;

	public void HideInventory(){
		inventoryUI.SetActive (false);
		transform.localScale = new Vector3(1, 1, 1);

	}

	public void ShowInventory(){
		inventoryUI.SetActive (true);
		transform.localScale = Vector3.zero; // inventory button is not visible
	}
}
