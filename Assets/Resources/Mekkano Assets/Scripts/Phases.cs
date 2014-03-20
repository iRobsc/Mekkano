using UnityEngine;
using System.Collections;

public class Phases : MonoBehaviour {

	public static int phase;

	public static void selectPhase(int currentPhase){
		phase = currentPhase;
		TouchHandler.unitSelection = false;
		TouchHandler.unitMovement = false;
		TouchHandler.unitAttacking = false;
		switch(currentPhase){
			case 1: 			
				TouchHandler.unitSelection = true;
				TouchHandler.unitMovement = true;
				break;
			case 2:
				TouchHandler.unitSelection = true;
				TouchHandler.unitAttacking = true;
				break;
			case 3:
				break;
		}
	}

	/*public static Mousepicking mousePicking;
	
	public Phases(Camera camera, Player currentPlayer, InputManager inputManager, AssetManager assetManager, Node rootNode, Grid grid){
		mousePicking = new Mousepicking(camera, currentPlayer, inputManager, assetManager, rootNode, grid);
		mousePicking.initialize();
	}
	
	public void selectPhase(String currentPhase){
		if (currentPhase == "movePhase"){
			mousePicking.unitSelection = true;
			mousePicking.unitMovement = true;
		}
		else if (currentPhase == "attackPhase"){
			
		}
		else if (currentPhase == "defensePhase"){
			
		}
	}*/
}
