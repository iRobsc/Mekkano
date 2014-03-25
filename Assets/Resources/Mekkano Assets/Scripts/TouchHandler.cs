using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

public class TouchHandler : MonoBehaviour {

	private Ray ray;
	private RaycastHit hit;
	private bool unitSelected = false;
	private Quaternion rotation;
	private bool moveUpdate = false, atkEachother = false;
	private Units selectedUnit;
	private List<Units> movingUnits = new List<Units>(), bothPlayerUnits;

	public static bool unitSelection = false;
	public static bool unitMovement = false;
	public static bool unitAttacking = false;
	public static bool switchPlayer = false;

	public static bool resetSelection = false;
	
	void Start(){
		bothPlayerUnits = Main.bothPlayerUnits;
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

	private void moveToTile(Tile targetTile, Tile tile){
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
			selectedUnit.setRange(false, true);
			selectedUnit = null;
			unitSelected = false;
		}
		resetSelection = false;
	}

	private void setUnitSelection(Units unit){
		unitSelected = true;
		selectedUnit = unit;
		selectedUnit.setSelectedTexture();
		if(unitAttacking == true) selectedUnit.setRange(true, false);
		else selectedUnit.setRange(true, true);
	}

	private void ignoreCollision(string tag, RaycastHit hit){
		/*List<GameObject> objects = new List<GameObject> ();
		objects.Add(GameObject.Find);
		foreach (GameObject obj in objects) {
			if(obj.GetComponent("Collider") && obj != gameObject){
				Physics.IgnoreCollision(hit.collider,obj.collider);
			}
		}*/
	}

	void Update(){

		if (resetSelection == true)resetUnitSelection();
		
		ray = Camera.main.ScreenPointToRay (Input.mousePosition);
		int layerMask = 1 << 8;

		if (Input.GetMouseButtonDown (0) && Physics.Raycast(ray, out hit, Mathf.Infinity, layerMask)) {
		
			Physics.IgnoreLayerCollision(0, 0, true);

			if (hit.collider.gameObject.name == "unitTile" && unitSelection == true){ // fråga till willebille går ej igenom units om units inte är med?

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
								else if (unit.playerIndex != Player.playerIndex &&  // if the unit is on the other team, then set it to the selected units attackTarget
								         TouchHandler.unitAttacking == true && selectedUnit != null){

									if(selectedUnit.inRange(unit.currentTile)){
										selectedUnit.targetLine(selectedUnit.currentTile, unit.currentTile, atkEachother);
										selectedUnit.faceTarget(selectedUnit, selectedUnit.attackTarget.currentTile);
										atkEachother = false;
									}

								}
							}
						}
					}
				}
			}

			if (hit.collider.gameObject.name == "tile" && unitSelected == true && unitMovement == true){
				foreach(Tile tile in Main.grid.getGrid()){
					if (tile != null && selectedUnit != null){
						if(tile.tile.transform == hit.collider.transform){ // searching for the selected geometry in the currentUnit's array
							if (tile.currentUnit == null && selectedUnit.inRange(tile)){
								moveToTile(tile,selectedUnit.currentTile);
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
			moveUnit();
		}
	}
}
