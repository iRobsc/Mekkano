using UnityEngine;
using System.Collections;

public class Main : MonoBehaviour {

	private static int gridXlength = 15, gridZlength = 10, xPos = 0, zPos = 0;
	private static float gridHeight = 0.1f;
	public static Player player1, player2;
	public static Grid grid;
	public static Player currentPlayer;
	private Units unit;

	void Start () {
		grid = gameObject.AddComponent<Grid>();
		grid.createGrid (xPos, zPos, gridXlength, gridZlength, gridHeight); 

		player1 = gameObject.AddComponent<Player> ();
		player1.createUnits (gridXlength, gridZlength, grid, gridHeight, true);

		player2 = gameObject.AddComponent<Player> ();
		player2.createUnits (gridXlength, gridZlength, grid, gridHeight, false);

		currentPlayer = player1;

	}

	void Update () {
	
	}
}
