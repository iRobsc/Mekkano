using UnityEngine;
using System.Collections;

public class TestUnit : Units {

	private Vector3 scaling = new Vector3 ((float)0.2, (float)0.2, (float)0.2);
	private string model    = "Mekkano Assets/Models/RangedUnit";
	private string texture  = "Mekkano Assets/Textures/tileA";
	public new int range = 4;

	public override void create(Tile tile, bool side) {
		createUnit (texture, model, tile, scaling, side, range);
	}
}
