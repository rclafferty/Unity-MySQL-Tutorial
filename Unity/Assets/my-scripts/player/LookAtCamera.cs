using UnityEngine;
using System.Collections;

public class LookAtCamera : MonoBehaviour {

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {

		// this is for Perspective cameras
		//this.transform.LookAt (Camera.main.transform);


		// this code is for Orthographic cameras
		transform.rotation = Camera.main.transform.rotation;
		transform.Rotate (0, 180, 0, Space.Self);
	
	}
}
