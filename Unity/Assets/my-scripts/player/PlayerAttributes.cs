using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class PlayerAttributes : MonoBehaviour {

	/*
Strength 
Dexterity 
Intelligence
Stamina 
Armor
magic resistance
attack speed
walk / run speed
mana points
health points
leaderboard rank (NEED league LEADERBOARD, league, contest)
level
experience
gold
runes
mana regeneration rate
health point regeneration rate

spell attributes: |mana cost |damage |cast time | attack type: melee / range
**************
skill tree
**************
**/

	public bool useUI = false;
	public int Strength = 0;
	public int Dexterity = 0;
	public int Intelligence = 0;
	public int Damage = 0;

	public Text textStrength;
	public Text textDexterity;
	public Text textIntelligence;
	public Text textDamage;

	public GameObject StatsUI;

	// Use this for initialization
	void Start () {
		GameObject manager = GameObject.FindGameObjectWithTag ("Manager");
		ManagerReferences refs = manager.GetComponent<ManagerReferences> ();
		StatsUI = refs.StatsUI;
		textStrength = refs.playerStatsTextStrength;
		textDexterity = refs.playerStatsTextDexterity;
		textIntelligence = refs.playerStatsTextIntelligence;
		textDamage = refs.playerStatsTextDamage;
	}
	
	// Update is called once per frame
	void Update () {
	
	}

	//todo: need to setup basic stats // warrior / mage / archer
	public void InitStatsForCharacterType(CharacterType type){
		switch (type) {
		case CharacterType.Warrior:
			// init player with warrior stats
			SetDefaultStatsForWarrior();
			break;
		case CharacterType.Assassin:
			// init player with assassin stats
			SetDefaultStatsForAssassin();
			break;
		case CharacterType.Archer:
			// init player with archer stats
			SetDefaultStatsForArcher();
			break;
		default: 
			break;
		}

		UpdateStatsUI ();
	}

	public void SetDefaultStatsForWarrior(){
		Strength = 10;
		Dexterity = 5;
		Intelligence = 1;
		Damage = 20;
	}

	public void SetDefaultStatsForAssassin(){
		Strength = 5;
		Dexterity = 10;
		Intelligence = 1;
		Damage = 10;
	}

	public void SetDefaultStatsForArcher(){
		Strength = 7;
		Dexterity = 7;
		Intelligence = 3;
		Damage = 5;
	}

	public void UpdateStatsUI() {
		if (useUI) {
			textStrength.text = "Strength : " + Strength.ToString ();
			textDexterity.text = "Dexterity: " + Dexterity.ToString ();
			textIntelligence.text = "Intelligence: " + Intelligence.ToString ();
			textDamage.text = "Damage : " + Damage.ToString ();
		}
	}

	public void ComputeStats(){
		// we want to get the inital values of the character type (warrior, assassin,etc..,
		// then we want to add the attributes of each equipped items
	
		InitStatsForCharacterType(GetComponent<PlayerHealth>().characterType);
		EquipItemScript equipItemScript = GetComponent<EquipItemScript> ();
		if (equipItemScript.HookRightHand.transform.childCount > 0) {
			Transform itemWeaponTransform = equipItemScript.HookRightHand.transform.GetChild (0);
			if (itemWeaponTransform != null) {
				GameObject itemWeapon = itemWeaponTransform.gameObject;
				if (itemWeapon != null) {
					ItemAttributes attributes = itemWeapon.GetComponent<ItemAttributes> ();
					if (attributes != null) {
						Strength += attributes.Strength;
						Dexterity += attributes.Dexterity;
						Intelligence += attributes.Intelligence;
						Damage += attributes.Damage;
					}
				}
			}
		}

		// todo: we need to repeat these steps for the other equipped items (shoulders, helmets, etc...)
		if (equipItemScript.HookHead.transform.childCount > 0) {
			Transform itemHelmetTransform = equipItemScript.HookHead.transform.GetChild (0);
			if (itemHelmetTransform != null) {
				GameObject itemHelmet = itemHelmetTransform.gameObject;
				if (itemHelmet != null) {
					ItemAttributes attributes = itemHelmet.GetComponent<ItemAttributes> ();
					if (attributes != null) {
						Strength += attributes.Strength;
						Dexterity += attributes.Dexterity;
						Intelligence += attributes.Intelligence;
						Damage += attributes.Damage;
					}
				}
			}
		}

		if (equipItemScript.HookLeftShoulder.transform.childCount > 0) {
			Transform itemShoulderLeftTransform = equipItemScript.HookLeftShoulder.transform.GetChild (0);
			if (itemShoulderLeftTransform != null) {
				GameObject itemShoulderLeft = itemShoulderLeftTransform.gameObject;
				if (itemShoulderLeft != null) {
					ItemAttributes attributes = itemShoulderLeft.GetComponent<ItemAttributes> ();
					if (attributes != null) {
						Strength += attributes.Strength;
						Dexterity += attributes.Dexterity;
						Intelligence += attributes.Intelligence;
						Damage += attributes.Damage;
					}
				}
			}
		}

		if (equipItemScript.HookRightShoulder.transform.childCount > 0) {
			Transform itemShoulderRightTransform = equipItemScript.HookRightShoulder.transform.GetChild (0);
			if (itemShoulderRightTransform != null) {
				GameObject itemShoulderRight = itemShoulderRightTransform.gameObject;
				if (itemShoulderRight != null) {
					ItemAttributes attributes = itemShoulderRight.GetComponent<ItemAttributes> ();
					if (attributes != null) {
						Strength += attributes.Strength;
						Dexterity += attributes.Dexterity;
						Intelligence += attributes.Intelligence;
						Damage += attributes.Damage;
					}
				}
			}
		}

		if (equipItemScript.HookLeftForeArm.transform.childCount > 0) {
			Transform itemShieldTransform = equipItemScript.HookLeftForeArm.transform.GetChild (0);
			if (itemShieldTransform != null) {
				GameObject itemShield = itemShieldTransform.gameObject;
				if (itemShield != null) {
					ItemAttributes attributes = itemShield.GetComponent<ItemAttributes> ();
					if (attributes != null) {
						Strength += attributes.Strength;
						Dexterity += attributes.Dexterity;
						Intelligence += attributes.Intelligence;
						Damage += attributes.Damage;
					}
				}
			}
		}


		// then update the User Interface for the stats
		UpdateStatsUI();

	}

	public int computeDamageDealt(){
		return this.Damage;
	}

	public int computeDamageReduction(){
		return (int)(Strength * 0.1); // todo: find a smarter algorithm to compute the damage reduction
	}
}








