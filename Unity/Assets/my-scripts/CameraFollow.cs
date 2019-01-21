using UnityEngine;
using System.Collections;

public class CameraFollow : MonoBehaviour {

	public GameObject Target; // the main chracter that the camera follows
	public float cameraSmoothing = 5.0f;
	//private Vector3 offsetVector = new Vector3 (0, 0.3f, 0);

	public GameObject initialCameraLookAtObject;

	//public Vector3 offset; // the distance between the main character and the camera
	// Use this for initialization
	void Start () {
		//if (Target != null) {
		//	offset = transform.position - Target.transform.position;
		//}

	}

	// Update is called once per frame
	void Update () {
		if (Target != null) {
			Vector3 targetCamera = Target.transform.position;// + offset + offsetVector;
			transform.position = Vector3.Lerp (transform.position, targetCamera, Time.deltaTime * cameraSmoothing);
		}
	}


	public void SetCameraOffset(Vector3 offset){
		//this.offset = this.transform.position - offset;
	}

	public void ComputeCameraShift(GameObject player) {
		this.transform.position = this.transform.position + (player.transform.position - initialCameraLookAtObject.transform.position);
	}

}
