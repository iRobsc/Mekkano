using UnityEngine;
using System.Collections;

public class TestUnit : Units {

	public TestUnit(){
		scaling = new Vector3 ((float)0.2, (float)0.2, (float)0.2);
		model    = "Mekkano Assets/Models/RangedUnit";
		texture  = "Mekkano Assets/Textures/tileA";
		hp = 3;
		baseDamage = 2;
		moveRange = 4;
		attackRange = 1;
		moveable = true;
		aura = false;
	}

	public override void create(Tile tile, bool side) {
		createUnit(tile, side);
	}
}
