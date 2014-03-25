using UnityEngine;
using System.Collections;

public class Phases : MonoBehaviour {

	public static int phase;

	public static void selectPhase(int currentPhase){
		phase = currentPhase;
		TouchHandler.unitSelection = false;
		TouchHandler.unitMovement = false;
		TouchHandler.unitAttacking = false;
		TouchHandler.switchPlayer = false;
		TouchHandler.resetSelection = true;

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
				TouchHandler.switchPlayer = true;
				TouchHandler.unitSelection = true;
				TouchHandler.unitAttacking = true;
				break;
		}

		if (TouchHandler.switchPlayer == true) TouchHandler.switchPlayers ();
	}
}
