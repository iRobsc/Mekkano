using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class TouchHandler : MonoBehaviour {

	private Ray ray;
	private RaycastHit hit;
	private bool unitSelected = false;
	private Quaternion rotation;
	private bool moveUpdate = false;
	private Units selectedUnit;
	private Player currentPlayer;
	private List<Units> movingUnits = new List<Units>(), bothPlayerUnits;

	public static bool unitSelection = false;
	public static bool unitMovement = false;
	public static bool unitAttacking = false;

	public static bool resetSelection = false;
	
	void Start(){
		currentPlayer = Main.currentPlayer;
		bothPlayerUnits = Main.bothPlayerUnits;
	}

	public static float calculateAngle(float firstPosX, float firstPosZ, float secondPosX, float secondPosZ){
		float deltaZ = firstPosZ - secondPosZ;
		float deltaX = firstPosX - secondPosX;
		float angle;
		return angle = (float)(Mathf.Atan2(-deltaX, -deltaZ));
	}

	public void moveToTile(Units unit, Tile targetTile, Tile tile){
		if (unit != null){
			selectedUnit.currentTile.currentUnit = null;
			selectedUnit.currentTile.tile.name = "tile";
			selectedUnit.currentTile = null;
			targetTile.currentUnit = selectedUnit;
			targetTile.tile.name = "unitTile";
			selectedUnit.currentTile = targetTile;
			selectedUnit.targetTile = targetTile;
			movingUnits.Add(selectedUnit);

			foreach(Units units in movingUnits){
				units.faceTarget(units, units.currentTile);
			}

			selectedUnit.setStandardTexture();
			selectedUnit.setRange(false, true);
			targetTile.setTexture(Tile.tileTextureB);
			
			selectedUnit = null;
		}
	}

	private void resetUnitSelection(){
		selectedUnit.setStandardTexture();
		selectedUnit.setRange(false, true);
		selectedUnit = null;
		unitSelected = false;
	}

	private void setUnitSelection(Units unit){
		unitSelected = true;
		selectedUnit = unit;
		selectedUnit.setSelectedTexture();
		if(Phases.phase == 1) selectedUnit.setRange(true, true);
		else selectedUnit.setRange(true, false);
	}
	
	void Update(){

		if (resetSelection == true && selectedUnit != null) {
			resetUnitSelection();
		}
		resetSelection = false;
		
		ray = Camera.main.ScreenPointToRay (Input.mousePosition);

		if (Input.GetMouseButtonDown (0) && Physics.Raycast(ray, out hit)) {

			if ((hit.collider.name == "unit" || hit.collider.gameObject.name == "unitTile") && unitSelection == true){ // fråga till willebille går ej igenom units om units inte är med?

				foreach(Units unit in bothPlayerUnits){ // change to all units
					if (unit != null){
						if((unit.unitModel.transform.GetChild(0) == hit.collider.transform && movingUnits.Contains(unit) == false) ||
						   (unit.currentTile.tile.transform == hit.collider.transform && movingUnits.Contains(unit) == false )){ // searching for the selected geometry in the currentUnit's array
							if (selectedUnit == unit){ // Resetting selected unit to null if the same unit is clicked twice
								resetUnitSelection();
							} else { // if another unit is clicked, make that one selected and remove selected to the previous one or attack if the unit is on the other team
								if (unit.playerIndex == Player.playerIndex){ 
									if (selectedUnit != null){ // if there's already an unit selection, then reset that selection
										resetUnitSelection();
									}
									setUnitSelection(unit);
								}
								else if (unit.playerIndex != Player.playerIndex &&  // if the unit is on the other team, then set it to the units target
								         TouchHandler.unitAttacking == true &&
								         selectedUnit != null){
										if(selectedUnit.inRange(unit.currentTile)){
											if(selectedUnit.attackTarget != null) {
												selectedUnit.attackTarget.setStandardTexture();
											}
											selectedUnit.attackTarget = unit;
											selectedUnit.faceTarget(selectedUnit, selectedUnit.attackTarget);
											selectedUnit.targetLine(selectedUnit.currentTile, selectedUnit.attackTarget.currentTile);
											unit.setSelectedTexture();
										}
								}
							}
						}
					}
				}
			}

			if (hit.collider.gameObject.name == "tile" && unitSelected == true && unitMovement == true){
				foreach(Tile tile in Main.grid.getGrid()){
					if (tile != null){
						if(tile.tile.transform == hit.collider.transform){ // searching for the selected geometry in the currentUnit's array
							if (tile.currentUnit == null && selectedUnit.inRange(tile)){
								moveToTile(selectedUnit,tile,selectedUnit.currentTile);
								unitSelected = false;
							}
						}
					}
				}
			}
		}

		if (movingUnits.Count == 0){
			moveUpdate = false;
		} else {
			moveUpdate = true;
		}

		if (moveUpdate == true){

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

					if (unit.currentTile.getTexture() != unit.currentTile.getTexture(Tile.tileTextureD) && 
					    unit.currentTile.getTexture() != unit.currentTile.getTexture(Tile.tileTextureC)){
						unit.currentTile.setTexture(Tile.tileTextureA);
					}
						else unit.currentTile.setTexture(Tile.tileTextureD);

					movingUnits.Remove(unit);
					break;
				}

				unit.unitModel.transform.position = new Vector3 (unitPos.x+moveX/Units.speed, 
				                                                 unitPos.y, 
				                                                 unitPos.z+moveZ/Units.speed);
			}
		}
	}
}
