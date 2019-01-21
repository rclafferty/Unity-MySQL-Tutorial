using UnityEngine;
using System.Collections;

public class Healthbar : MonoBehaviour {

	public float maxValue = 100.0f;
	public Color minColor = Color.red;
	public Color maxColor = Color.green;

	private SpriteRenderer rend;

	public GameObject Player;
	public float initialLength = 0.2f;
	public float currentLength = 0.2f;

	// Use this for initialization
	void Start () {
		rend = GetComponent<SpriteRenderer> ();
		currentLength = initialLength;
		maxValue = (float)Player.GetComponent<PlayerHealth> ().startingHealth;
	}
	
	// Update is called once per frame
	void Update () {
		float fraction = (float)Player.GetComponent<PlayerHealth> ().currentHealth / maxValue;
		rend.color = Color.Lerp (
			minColor, 
			maxColor,
			Mathf.Lerp (0, 1,  Player.GetComponent<PlayerHealth> ().currentHealth / maxValue  ));

		this.transform.localScale = new Vector3 (initialLength * fraction, transform.localScale.y, transform.localScale.z);

		// healthbar needs to look at the camera at all times:
		transform.LookAt(Camera.main.transform, -Vector3.up);
	}
}
