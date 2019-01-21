using UnityEngine;
using System.Collections;
using UnityStandardAssets.CrossPlatformInput;
using DigitalRuby.PyroParticles;
using UnityEngine.Networking;

public class PlayerShoot : NetworkBehaviour {


	RaycastHit shootHit;
	Ray shootRay;
	int shootableMask;
	public bool isShooting = false;
	public int damagePoints = 10;

	public bool isEnabled = true;

	public GameObject[] projectilePrefabs;
	private GameObject selectedProjectilePrefab;
	private GameObject currentPrefabObject;
	FireBaseScript currentPrefabScript;
	public GameObject projectileSpawnPoint;

	public PlayerHealth healthScript;

	// Use this for initialization
	void Start () {

		InitializeProjectile ();
		healthScript = GetComponent<PlayerHealth> ();

	}

	// Update is called once per frame
	void Update () {
		if (!isLocalPlayer)
			return;
		
		/*#if !MOBILE_INPUT
		if (Input.GetButtonDown ("Fire1") && isShooting == false && isEnabled == true) {
			CmdShoot ();

		} 
		#else
		if(CrossPlatformInputManager.GetAxisRaw("Mouse X") != 0 || CrossPlatformInputManager.GetAxisRaw("Mouse Y") != 0){
		CmdShoot("");
		}
		#endif*/

	}

	[Command]
	public void CmdShoot(string itemPath){
		if (healthScript.currentHealth <= 0) {
			return;
		}
		isShooting = true;

		if (string.IsNullOrEmpty (itemPath)) {
			SpawnProjectile (selectedProjectilePrefab);
		} else {
			GameObject projectile = Instantiate (Resources.Load (itemPath)) as GameObject;
			SpawnProjectile (projectile);
		}

		Invoke ("StopShooting", 0.15f);

	}

	void StopShooting(){
		isShooting = false;
	}

	public void DisableShooting(){
		isEnabled = false;
	}

	void InitializeProjectile(){
		int selected = Random.Range (1, 1000) % projectilePrefabs.Length;
		selectedProjectilePrefab = projectilePrefabs [selected];
	}

	void SpawnProjectile(GameObject projectile){
		currentPrefabObject = GameObject.Instantiate (projectile);
		currentPrefabObject.transform.position = projectileSpawnPoint.transform.position;
		currentPrefabObject.transform.rotation = transform.rotation;
		NetworkServer.Spawn (currentPrefabObject);
		currentPrefabObject.GetComponent<FireProjectileScript> ().ownerName = transform.name;
	}
}













