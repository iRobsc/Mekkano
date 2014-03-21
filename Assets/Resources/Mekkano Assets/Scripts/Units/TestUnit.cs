using UnityEngine;
using System.Collections;

public class TestUnit : Units {

	private Vector3 scaling = new Vector3 ((float)0.2, (float)0.2, (float)0.2);
	private string model    = "Mekkano Assets/Models/RangedUnit";
	private string texture  = "Mekkano Assets/Textures/tileA";
	public new int moveRange = 15;
	public new int attackRange = 1;

	public override void create(Tile tile, bool side) {
		createUnit (texture, model, tile, scaling, side, moveRange, attackRange);
	}
}
