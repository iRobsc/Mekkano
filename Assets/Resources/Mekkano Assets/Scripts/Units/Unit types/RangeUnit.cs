using UnityEngine;
using System.Collections;

public class RangeUnit : Units {

	public RangeUnit(){
		scaling = new Vector3 ((float)0.2, (float)0.2, (float)0.2);
		model    = "Mekkano Assets/Models/RangedUnit";
		texture  = "Mekkano Assets/Textures/tileA";
		hp = 3;
		baseDamage = 1;
		moveRange = 2;
		attackRange = 3;
		moveable = true;
		aura = false;
	}
	
	public override void create(Tile tile, bool side) {
		createUnit(tile, side);
	}
}