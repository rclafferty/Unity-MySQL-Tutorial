using UnityEngine;
using System.Collections;

public class ColliderDefense : MonoBehaviour {

	public GameObject owner;


	public ManagerReferences getManagerRefs() {
		GameObject manager = GameObject.FindGameObjectWithTag("Manager");
		return manager.GetComponent<ManagerReferences> ();
	}

	void Start () {
		if (owner == null) {
			ManagerReferences refs = getManagerRefs ();
			owner = refs.localPlayer;
		}

	}

}
