using UnityEngine;
using System.Collections;

public class TestUnit : Units {

	public TestUnit(){
		scaling = new Vector3 ((float)0.2, (float)0.2, (float)0.2);
		model    = "Mekkano Assets/Models/RangedUnit";
		texture  = "Mekkano Assets/Textures/tileA";
		hp = 2;
		damage = 2;
		moveRange = 15;
		attackRange = 15;
	}

	public override void create(Tile tile, bool side) {
		createUnit (tile, side);
	}
}
