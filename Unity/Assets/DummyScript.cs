using UnityEngine;
using System.Collections;

public class DummyScript : MonoBehaviour {

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}

	public void OnCollisionEnter(Collision col) {
		Debug.Log ("Dummy got hit by " + col.gameObject.name);
	}
}
