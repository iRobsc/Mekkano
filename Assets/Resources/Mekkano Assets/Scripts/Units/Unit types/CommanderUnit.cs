using UnityEngine;
using System.Collections;

public class CommanderUnit : Units {

	private int commanderBuffDmg = 2;

	public CommanderUnit(){
		scaling = new Vector3 ((float)0.2, (float)0.2, (float)0.2);
		model    = "Mekkano Assets/Models/CommanderUnit";
		texture  = "Mekkano Assets/Textures/tileA";
		hp = 8;
		baseDamage = 5;
		moveRange = 1;
		attackRange = 1;
		buffRange = 1;
		moveable = true;
		aura = true;
	}

	public override void buff(Units unit, bool remove){
		if (remove && unit.hasBuff) unit.damageBuff -= commanderBuffDmg;
		else if (unit.hasBuff == false) unit.damageBuff += commanderBuffDmg;

		unit.calculateDamage();

		if (remove) unit.hasBuff = false; 
		else unit.hasBuff = true;
	}

	public override void create(Tile tile, bool side) {
		createUnit(tile, side);
	}
}