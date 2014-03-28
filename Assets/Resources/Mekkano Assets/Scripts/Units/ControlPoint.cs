using UnityEngine;
using System.Collections;

public class ControlPoint : Units {

	public ControlPoint(){
		scaling = new Vector3 ((float)0.2, (float)0.2, (float)0.2);
		model    = "Mekkano Assets/Models/ControlPoint";
		texture  = "Mekkano Assets/Textures/tileA";
		range = 1;
		playerIndex = 0;
		moveable = false;
		aura = true;
	}

	public virtual void create(Tile tile) {
		createObject(tile);
	}
}
