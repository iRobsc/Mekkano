using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

public class Main : MonoBehaviour {

	public static int gridXlength = 15, gridZlength = 10, xPos = 0, zPos = 0;
	public static float gridHeight = 0.1f;
	public static Player player1, player2;
	public static Grid grid;
	public static GuiScript gui;
	public static CameraScript cam;
	public static Player currentPlayer;
	public static List<Units> allUnits;
	public static List<ControlPoint> controlPoints;

	void Start () {
		gui = gameObject.AddComponent<GuiScript>();

		cam = gameObject.AddComponent<CameraScript>();

		grid = gameObject.AddComponent<Grid>();
		grid.createGrid (xPos, zPos, gridXlength, gridZlength, gridHeight); 

		player1 = gameObject.AddComponent<Player> ();
		player1.createUnits (grid, gridHeight, true);

		player2 = gameObject.AddComponent<Player> ();
		player2.createUnits (grid, gridHeight, false);

		controlPoints = new List<ControlPoint>();
		controlPoints.Add(gameObject.AddComponent<ControlPoint>());
		controlPoints.Add(gameObject.AddComponent<ControlPoint>());
		controlPoints[0].create(grid.getTile(7,7));
		controlPoints[0].buffType = 0;
		controlPoints[1].create(grid.getTile(7,2));
		controlPoints[1].buffType = 1;
		
		allUnits = new List<Units> ();
		allUnits.AddRange(player1.units.Cast<Units>().ToList());
		allUnits.AddRange(player2.units.Cast<Units>().ToList());
		allUnits.AddRange(controlPoints.Cast<Units>().ToList());
	
		currentPlayer = player1;
		Player.playerIndex = 1;
		Phases.selectPhase(1);
	}

	/*void OnGUI(){

		GUI.Box (new Rect (150,10,100,40), "Phase "+Phases.phase);

		if (GUI.Button(new Rect(10,10,120,40),"Next Phase")){
			if (Phases.phase < 3){
				Phases.selectPhase(Phases.phase+1);
			} else {
				Phases.selectPhase(1);
				Units.engageAttacks(allUnits);
			}
		}
	}*/

	void Update () {
	
	}
}
