using UnityEngine;
using System.Collections;

public class MeleeUnit : Units {

	public MeleeUnit(){
		scaling = new Vector3 ((float)0.2, (float)0.2, (float)0.2);
		model    = "Mekkano Assets/Models/MeleeUnit";
		texture  = "Mekkano Assets/Textures/tileA";
		hp = 3;
		baseDamage = 3;
		moveRange = 3;
		attackRange = 1;
		moveable = true;
		aura = false;
	}
	
	public override void create(Tile tile, bool side) {
		createUnit(tile, side);
	}
}