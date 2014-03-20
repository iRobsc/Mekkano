using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

public class Main : MonoBehaviour {

	public static int gridXlength = 15, gridZlength = 10, xPos = 0, zPos = 0;
	private static float gridHeight = 0.1f;
	public static Player player1, player2;
	public static Grid grid;
	public static Player currentPlayer;
	public static List<Units> bothPlayerUnits;
	private Units unit;

	void Start () {
		grid = gameObject.AddComponent<Grid>();
		grid.createGrid (xPos, zPos, gridXlength, gridZlength, gridHeight); 

		player1 = gameObject.AddComponent<Player> ();
		player1.createUnits (gridXlength, gridZlength, grid, gridHeight, true);

		player2 = gameObject.AddComponent<Player> ();
		player2.createUnits (gridXlength, gridZlength, grid, gridHeight, false);

		bothPlayerUnits = new List<Units> ();
		bothPlayerUnits.AddRange (player1.units.Cast<Units> ().ToList ());
		bothPlayerUnits.AddRange (player2.units.Cast<Units> ().ToList ());

		currentPlayer = player1;
		Player.playerIndex = 1;
		Phases.selectPhase(1);
	}

	void OnGUI(){

		GUI.Box (new Rect (150,10,100,40), "Phase "+Phases.phase);

		if (GUI.Button(new Rect(10,10,120,40),"Next Phase")){
			if (Phases.phase < 3){
				Phases.selectPhase(Phases.phase+1);
				TouchHandler.resetSelection = true;
			} else {
				Phases.selectPhase(1);
			}
		}
	}

	void Update () {
	
	}
}
