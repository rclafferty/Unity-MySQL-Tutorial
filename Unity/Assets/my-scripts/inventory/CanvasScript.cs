using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class CanvasScript : MonoBehaviour {

	public Canvas canvas;
	// Use this for initialization
	void Start () {
		canvas.worldCamera = Camera.main;
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}
}
