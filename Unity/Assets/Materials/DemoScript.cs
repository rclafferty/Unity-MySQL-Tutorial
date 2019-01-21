using UnityEngine;
using System.Collections;

public class DemoScript : MonoBehaviour {

	public bool isBlocking = true;

	// Use this for initialization
	void Start () {
		if (isBlocking) {
			GetComponent<Animator>().SetTrigger ("block");
		}
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}
}
