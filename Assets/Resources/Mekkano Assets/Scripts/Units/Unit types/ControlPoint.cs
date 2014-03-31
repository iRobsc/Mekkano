using UnityEngine;
using System.Collections;

public class ControlPoint : Units {

	private int controlPointDamage = 5, controlPointMovement = 1;

	public ControlPoint(){
		scaling = new Vector3 ((float)0.2, (float)0.2, (float)0.2);
		model    = "Mekkano Assets/Models/ControlPoint";
		texture  = "Mekkano Assets/Textures/tileA";
		buffRange = 1;
		playerIndex = 0;
		moveable = false;
		aura = true;
		buffConquest = new int[2];
	}

	public override void buff(Player buffingPlayer, int buffType, bool remove){
		foreach(Units unit in buffingPlayer.units) {
			if (unit != null){
				if(buffType == 0){
					if(remove) unit.damageBuff -= controlPointDamage;
					else unit.damageBuff += controlPointDamage;
					unit.calculateDamage();
				} else if(buffType == 1){
					if(remove) unit.movementBuff -= controlPointMovement;
					else unit.movementBuff += controlPointMovement;
					unit.calculateMovement();
				}
			}
		}
		if (remove) buffingPlayer.hasBuff[buffType] = false; 
		else buffingPlayer.hasBuff[buffType] = true;
	}

	public override void create(Tile tile) {
		createObject(tile);
	}
}
