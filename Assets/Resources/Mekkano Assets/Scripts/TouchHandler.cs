using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

public class TouchHandler : MonoBehaviour {

	private Ray ray;
	private RaycastHit hit;
	private bool unitSelected = false;
	private Quaternion rotation;
	private bool moveUpdate = false;
	private Units selectedUnit;
	private List<Units> movingUnits = new List<Units>(), allUnits;
	private Grid grid;

	public static bool unitSelection = false;
	public static bool unitMovement = false;
	public static bool unitAttacking = false;
	public static bool switchPlayer = false;

	public static bool resetSelection = false;
	
	void Start(){
		grid = Main.grid;
		allUnits = Main.allUnits;
	}

	public static float calculateAngle(float firstPosX, float firstPosZ, float secondPosX, float secondPosZ){
		float deltaZ = firstPosZ - secondPosZ;
		float deltaX = firstPosX - secondPosX;
		return (float)(Mathf.Atan2(-deltaX, -deltaZ));
	}

	public static void switchPlayers(){
		//move camera
		//move UI
		if (Player.playerIndex == 1) {
			Main.currentPlayer = Main.player2;
			Player.playerIndex = 2;
		} else {
			Main.currentPlayer = Main.player1;
			Player.playerIndex = 1;
		}
	}

	private void buffConquest(List<Units> buffSources, Units selectedUnit, bool tileAddition){
		foreach(Units buffSource in buffSources){
			if(buffSource.playerIndex == 0){
				if(tileAddition == true) { // add 1 unit to the buff range if it enters it, else remove 1 unit
					buffSource.buffConquest[selectedUnit.playerIndex - 1] += 1;
				} else buffSource.buffConquest[selectedUnit.playerIndex - 1] -= 1;
			} else if(selectedUnit.playerIndex == buffSource.playerIndex && selectedUnit.hasBuff){
				if(tileAddition == true){
					buffSource.buff(selectedUnit.owner, buffSource.buffType, false);
				} else buffSource.buff(selectedUnit.owner, buffSource.buffType, false);
			}
		}
	}

	private void configureBuffs(List<Units> buffSources){
		foreach(Units buffSource in buffSources){
			if(buffSource.playerIndex == 0){
				int p1Amount = buffSource.buffConquest[0];
				int p2Amount = buffSource.buffConquest[1];

				if(p1Amount > p2Amount && Main.player1.hasBuff[buffSource.buffType] == false) { // if there are more player1 units, add the buff to player1
					buffSource.buff(Main.player1, buffSource.buffType, false);
				if (Main.player2.hasBuff[buffSource.buffType]) buffSource.buff(Main.player2, buffSource.buffType, true); // if player 2 has a buff, remove it
				}
				else if(p1Amount < p2Amount && Main.player2.hasBuff[buffSource.buffType] == false) { // same as the recent one but the opposite
					buffSource.buff(Main.player2, buffSource.buffType, false);
					if (Main.player1.hasBuff[buffSource.buffType]) buffSource.buff(Main.player1, buffSource.buffType, true);
				} else if(p1Amount == p2Amount){
					if(Main.player1.hasBuff[buffSource.buffType]) buffSource.buff(Main.player1, buffSource.buffType, true); // if there is an equal amount of units in the range, then remove both's buffs
					if(Main.player2.hasBuff[buffSource.buffType]) buffSource.buff(Main.player2, buffSource.buffType, true);
				}
			}
		}
	}

	private void moveToTile(Tile targetTile, Tile tile){
		targetTile.currentUnit = selectedUnit;
		targetTile.tileMesh.name = "collisionTile";

		if(selectedUnit.currentTile.buffSources.Count > 0) {
			buffConquest(selectedUnit.currentTile.buffSources, selectedUnit, false);
		}
		if(targetTile.buffSources.Count > 0){
			buffConquest(targetTile.buffSources, selectedUnit, true);
		}
		if(selectedUnit.currentTile.buffSources != targetTile.buffSources){
			configureBuffs(selectedUnit.currentTile.buffSources);
		}
		configureBuffs(targetTile.buffSources);

		selectedUnit.currentTile.currentUnit = null;
		selectedUnit.currentTile.tileMesh.name = "tile";
		selectedUnit.currentTile = null; 
		selectedUnit.currentTile = targetTile;
		selectedUnit.targetTile = targetTile;
		movingUnits.Add(selectedUnit);
	
		foreach(Units units in movingUnits){
			units.faceTarget(units, units.currentTile);
		}	
		selectedUnit.setStandardTexture();
		grid.hideRange(selectedUnit.rangeTiles);
		targetTile.setTexture(Tile.tileTextureB);

		Main.currentPlayer.movementPoints -= 1;
		if(Main.currentPlayer.movementPoints == 0){
			Phases.selectPhase(Phases.phase+1);
			Main.currentPlayer.movementPoints = Main.currentPlayer.movementPointBase;
		}
		selectedUnit = null;
	}

	private void moveUnit(){
		foreach(Units unit in movingUnits){
			Vector3 unitPos = unit.unitModel.transform.position;
			
			float deltaZ = unit.currentTile.getZindex() - unitPos.z;
			float deltaX = unit.currentTile.getXindex() - unitPos.x;
			float angle = (Mathf.Atan2(deltaZ, deltaX));
			
			float moveX = (float) Mathf.Cos((float)angle);
			float moveZ = (float) Mathf.Sin((float)angle);
			
			if (Mathf.Abs(unitPos.x - unit.currentTile.getXindex()) <= Mathf.Abs(moveX/Units.speed) &&
			    Mathf.Abs(unitPos.z - unit.currentTile.getZindex()) <= Mathf.Abs(moveZ/Units.speed)){
				
				unit.transform.position = new Vector3(unit.currentTile.getXindex(), unitPos.y, unit.currentTile.getZindex());
				
				if (unit.currentTile.getTexture() == unit.currentTile.getTexture(Tile.tileTextureB)){
					unit.currentTile.setTexture(Tile.tileTextureA);
				}
				
				movingUnits.Remove(unit);
				break;
			}
			
			unit.unitModel.transform.position = new Vector3 (unitPos.x+moveX/Units.speed, 
			                                                 unitPos.y, 
			                                                 unitPos.z+moveZ/Units.speed);
		}
	}

	private void resetUnitSelection(){
		if (selectedUnit != null){
			selectedUnit.setStandardTexture();
			if (selectedUnit.rangeTiles != null) grid.hideRange(selectedUnit.rangeTiles);
			else if (selectedUnit.buffTiles != null) grid.hideRange(selectedUnit.buffTiles);
			selectedUnit = null;
			unitSelected = false;
		}
		resetSelection = false;
	}

	private void setUnitSelection(Units unit){
		resetUnitSelection();
		unitSelected = true;
		selectedUnit = unit;
		selectedUnit.setSelectedTexture();

		if(unit.buffRange > 0) grid.setRange(selectedUnit.currentTile, selectedUnit.buffRange, true);

		if(unit.moveable == true) {
			if(unitAttacking == true)
				grid.setRange(selectedUnit.currentTile, selectedUnit.attackRange, false);
			else
				grid.setRange(selectedUnit.currentTile, selectedUnit.movementRange, false);
		}
		grid.showRange(selectedUnit);
	}

	void Update(){

		if (resetSelection == true)resetUnitSelection();
		
		ray = Camera.main.ScreenPointToRay (Input.mousePosition);
		int layerMask = 1 << 8;

		if (Input.GetMouseButtonDown (0) && Physics.Raycast(ray, out hit, Mathf.Infinity, layerMask)) {
		
			Physics.IgnoreLayerCollision(0, 0, true);

			unitSelecting();
			tileSelecting();
		}

		if (movingUnits.Count == 0){
			moveUpdate = false;
		} else {
			moveUpdate = true;
		}

		if (moveUpdate == true){
			moveUnit();
		}
	}

	private void unitSelecting(){
		if (hit.collider.gameObject.name == "collisionTile" && unitSelection == true){ 
			foreach(Units unit in allUnits){
				if (unit != null){
					if(unit.currentTile.tileMesh.transform == hit.collider.transform && movingUnits.Contains(unit) == false){ // searching for the selected geometry in the currentUnit's array
						if (selectedUnit == unit){ // Resetting selected unit to null if the same unit is clicked twice
							resetUnitSelection();
						} else { // if another unit is clicked, make that one selected and remove selected to the previous one or attack if the unit is on the other team
							if (unit.playerIndex == Player.playerIndex){
								setUnitSelection(unit);
							} else if (unit.playerIndex != Player.playerIndex && unit.playerIndex != 0 && // if the unit is on the other team, then set it to the selected units attackTarget
							           TouchHandler.unitAttacking == true && selectedUnit != null){
								if(selectedUnit.inRange(unit.currentTile)){
									if(selectedUnit.attackTarget == unit) {
										selectedUnit.attackTarget.resetAtkEachother(selectedUnit,selectedUnit.attackTarget);
										selectedUnit.attackTarget = null;
										Destroy(selectedUnit.attackLine);
									} else {
										selectedUnit.targetLine(selectedUnit.currentTile, unit.currentTile);
										selectedUnit.faceTarget(selectedUnit, selectedUnit.attackTarget.currentTile);
									}
								}
							} else if (unit.playerIndex == 0) { // if the object is a neutral object like a control point
								setUnitSelection(unit);
							}
						}
					}
				}
			}
		}
	}

	private void tileSelecting(){
		if (hit.collider.gameObject.name == "tile" && unitSelected == true && unitMovement == true){
			if(selectedUnit.moveable == true){
				foreach(Tile tile in Main.grid.getGrid()){
					if (tile != null){
						if(tile.tileMesh.transform == hit.collider.transform){ // searching for the selected geometry in the currentUnit's array
							if (tile.currentUnit == null && selectedUnit.inRange(tile)){
								moveToTile(tile,selectedUnit.currentTile);
								unitSelected = false;
							}
						}
					}
				}
			}	
		}
	}

}
