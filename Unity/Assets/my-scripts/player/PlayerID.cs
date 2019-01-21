using UnityEngine;
using System.Collections;
using UnityEngine.Networking;

public class PlayerID : NetworkBehaviour {

	[SyncVar]
	public string playerUniqueName;

	private NetworkInstanceId playerNetID;

	public override void OnStartLocalPlayer(){
		GetNetIdentity();
		SetIdentity ();
		SetCameraTarget ();
	}

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
		if (transform.name == "" || transform.name.StartsWith("MAN1Prefab")) {
			SetIdentity ();
		}
	
	}

	[Client]
	void GetNetIdentity(){
		playerNetID = GetComponent<NetworkIdentity> ().netId;
		CmdTellServerMyIdentity( MakeUniqueIdentity() );
	}

	void SetIdentity(){
		if (!isLocalPlayer) {
			this.transform.name = playerUniqueName;
		} else {
			// case when it is the local client player
			// we need to create the ID
			transform.name = MakeUniqueIdentity();
			GameObject manager = GameObject.FindGameObjectWithTag ("Manager");
			ManagerReferences refs = manager.GetComponent<ManagerReferences> ();
			refs.localPlayer = this.gameObject;
		}
	}

	string MakeUniqueIdentity(){
		return "Player" + playerNetID.ToString ();
	}

	[Command]
	void CmdTellServerMyIdentity(string name){
		playerUniqueName = name;
	}

	void SetCameraTarget(){
		if (isLocalPlayer) {
			Camera.main.GetComponent<CameraFollow> ().Target = this.gameObject;
			//Camera.main.GetComponent<CameraFollow> ().SetCameraOffset (this.gameObject.transform.position);
		}
	}
}
