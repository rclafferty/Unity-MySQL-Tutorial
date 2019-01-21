using UnityEngine;
using System.Collections;
using UnityStandardAssets.CrossPlatformInput;
using UnityEngine.Networking;


public class PlayerBehaviorScript : NetworkBehaviour {

	public float speed = 2.0f;
	Vector3 movement;
	Animator animator;
	public bool isAttacking = false;
	public bool isBlocking = false;

	float initialPosY;

	Rigidbody playerRigidBody;
	int floorMask;
	float camRayLength = 500.0f;

	public Renderer myRenderer;
	public Material[] materials;
	public Material currentMaterial;

	public bool isEnabled = true;

	PlayerShoot playerShootScript;

	void Awake() {
		playerRigidBody = GetComponent<Rigidbody> ();
		floorMask = LayerMask.GetMask ("Floor");
	}

	// Use this for initialization
	void Start () {
		animator = GetComponent<Animator> ();	
		initialPosY = transform.position.y;

		InitializeCostume ();
	}

	public override void OnStartLocalPlayer ()
	{
		
		base.OnStartLocalPlayer ();

		GameObject.FindGameObjectWithTag ("Manager").GetComponent<ManagerReferences> ().LocalPlayerSpawned();
		Camera.main.GetComponent<CameraFollow> ().ComputeCameraShift (this.gameObject);
		playerShootScript = GetComponent<PlayerShoot> ();
	}
	
	void FixedUpdate(){
		if (!isLocalPlayer || !isEnabled)
			return;
		
		float h = CrossPlatformInputManager.GetAxisRaw ("Horizontal");
		float v = CrossPlatformInputManager.GetAxisRaw ("Vertical");

		movement.Set (h, 0f, v);
		movement = movement.normalized * Time.fixedDeltaTime * speed;

		animator.SetFloat ("speed", v);
		animator.SetFloat("strafe", -h);


		if (Input.GetButtonDown ("Jump")) {
			if (isAttacking == false) {
				//Debug.Log (GetComponent<EquipItemScript> ().currentWeaponType.ToString ());
				//animator.SetTrigger ("attack"); // set trigger does not sync over network, using SetBool instead
				if (GetComponent<EquipItemScript> ().currentWeaponType == ItemType.Weapon) {
					Invoke ("AttackAnimationOver", 1.0f);
					isAttacking = true;
					animator.SetBool ("attacking", true);
					Invoke ("stopAnimationAttacking", 0.09f);
				} else if (GetComponent<EquipItemScript> ().currentWeaponType == ItemType.WeaponArchery) {
					Invoke ("AttackAnimationOver", 1.0f);
					isAttacking = true;
					animator.SetBool ("attackingBow", true);
					Invoke ("shootArrow", 0.8f);
					Invoke ("stopAnimationAttackingBow", 0.09f);
				}
				//CmdTakeDamage (50);

				/*if (isServer) {
					GameObject obj = GameObject.Find (GetComponent<PlayerID> ().playerUniqueName);
					obj.GetComponent<PlayerHealth> ().TakeDamage (50);
				} else {
					GameObject obj = GameObject.Find (GetComponent<PlayerID> ().playerUniqueName);
					obj.GetComponent<PlayerHealth> ().CmdTakeDamage (50);
				}*/

				//GameObject obj = GameObject.Find (GetComponent<PlayerID> ().playerUniqueName);
				//obj.GetComponent<PlayerHealth> ().CmdTakeDamage (50);
			}
		}

		if (Input.GetKey(KeyCode.LeftControl)) {
				//animator.SetTrigger ("turn");
				animator.SetBool ("turning", true);
				Invoke ("stopAnimationTurning", 0.09f);
		}

		if (Input.GetKeyDown("1")) {
			if (isAttacking == false) {
				//animator.SetTrigger ("combo1");
				Invoke ("AttackAnimationOver", 0.6f);
				animator.SetBool ("combination1", true);
				Invoke ("stopAnimationCombo1", 0.09f);
				isAttacking = true;
			}
		}
			
		if(animator.GetCurrentAnimatorStateInfo (0).IsName("idlewalk")) {
			transform.position = new Vector3(transform.position.x, initialPosY, transform.position.z);
			if (animator.GetFloat ("speed") == -1) {
				isBlocking = true;
				Debug.Log (this.gameObject.name + " is blocking");
			} else {
				isBlocking = false;
			}
		}

		#if !MOBILE_INPUT
		if (Input.GetKeyDown ("2") && playerShootScript.isShooting == false && isEnabled == true && isLocalPlayer) {
			playerShootScript.CmdShoot ("");
		} 
		#else
		if(CrossPlatformInputManager.GetAxisRaw("Mouse X") != 0 || CrossPlatformInputManager.GetAxisRaw("Mouse Y") != 0){
		CmdShoot("");
		}
		#endif

	}

	public void shootArrow() {
		GetComponent<PlayerShoot> ().CmdShoot ("MyPrefabs/gear/ArrowPrefab");
	}

	public void AttackAnimationOver(){
		isAttacking = false;
	}

	public void stopAnimationAttacking(){
		animator.SetBool ("attacking", false);
	}

	public void stopAnimationAttackingBow(){
		animator.SetBool ("attackingBow", false);
	}

	public void stopAnimationTurning(){
		animator.SetBool ("turning", false);
	}

	public void stopAnimationCombo1(){
		animator.SetBool ("combination1", false);
	}



	void InitializeCostume(){
		int nb = Random.Range (0, 1000) % materials.Length;
		gameObject.GetComponentInChildren<Renderer> ().material = materials [nb] as Material;
		myRenderer.material = materials [nb] as Material;
		currentMaterial = materials [nb] as Material;
	}

	[ClientRpc]
	public void RpcTriggerAnimation(string animationName)
	{
		//if (isLocalPlayer)
			//return;
		GetComponent<Animator> ().Play (animationName);
	}

	[Command]
	void CmdTriggerAnimation(string animationName)
	{
		RpcTriggerAnimation(animationName);
	}

	[Command]
	public void CmdTellServerWhoGotShot(string uniqueID, int damage){
		GameObject obj = GameObject.Find (uniqueID);
		obj.GetComponent<PlayerHealth> ().TakeDamage (damage);
		//Debug.Log ("player health: " + obj.GetComponent<PlayerHealth> ().currentHealth.ToString ());
	}

	void IncreaseNumberOfKillsForPlayerWithName(string playerUniqueName){
		GameObject obj = GameObject.Find (playerUniqueName);
		obj.GetComponent<PlayerKills>().KillsCount++;	
	}

	[Command]
	public void CmdTakeDamage(int amount) {
		Debug.Log ("cmd taking dmg");
		this.GetComponent<PlayerHealth> ().TakeDamage (amount);
	}

	[Command]
	public void CmdDestroyObject(GameObject obj){
		Destroy (obj);
	}

	public void ShootFirebolt(){
		if (playerShootScript == null) {
			playerShootScript = GetComponent<PlayerShoot> ();
		}
		#if !MOBILE_INPUT
		if (playerShootScript.isShooting == false && isEnabled == true) {
			GameObject p = GameObject.FindGameObjectWithTag("Manager").GetComponent<ManagerReferences>().localPlayer;
			p.GetComponent<PlayerShoot>().CmdShoot ("");
		} 
		#else
		if(CrossPlatformInputManager.GetAxisRaw("Mouse X") != 0 || CrossPlatformInputManager.GetAxisRaw("Mouse Y") != 0){
		CmdShoot("");
		}
		#endif
	}

}












