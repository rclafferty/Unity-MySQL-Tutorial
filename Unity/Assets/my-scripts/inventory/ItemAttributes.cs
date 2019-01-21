using UnityEngine;
using System.Collections;

public class Attributes {
	public int Strength;
	public int Dexterity;
	public int Intelligence;
	public int Damage;

	public void cloneAttributesFrom(ItemAttributes attributes){
		Strength = attributes.Strength;
		Dexterity = attributes.Dexterity;
		Intelligence = attributes.Intelligence;
		Damage = attributes.Damage;
	}

	public void Fill(int strength, int dexterity, int intelligence, int damage){
		this.Strength = strength;
		this.Dexterity = dexterity;
		this.Intelligence = intelligence;
		this.Damage = damage;
	}

	/*public Attributes(){
		this.Strength = 0;
		this.Dexterity = 0;
		this.Intelligence = 0;
		this.Damage = 0;
	}*/
}

public class ItemAttributes : MonoBehaviour {
	public int Strength;
	public int Dexterity;
	public int Intelligence;
	public int Damage;

	// ADD MORE ATTRIBUTES HERE....
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
*/
	public void cloneAttributesFrom(ItemAttributes attributes){
		Strength = attributes.Strength;
		Dexterity = attributes.Dexterity;
		Intelligence = attributes.Intelligence;
		Damage = attributes.Damage;
	}

	public void cloneAttributesFrom(Attributes attributes){
		Strength = attributes.Strength;
		Dexterity = attributes.Dexterity;
		Intelligence = attributes.Intelligence;
		Damage = attributes.Damage;
	}



}
