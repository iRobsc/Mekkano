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
				Vector3 unitPos = units.unitModel.transform.position;
				float deltaZ = units.currentTile.getZ() - unitPos.z;
				float deltaX = units.currentTile.getX() - unitPos.x;
				float angle = (float)Mathf.Atan2((float)deltaX, (float)deltaZ);
				rotation = Quaternion.AngleAxis(Mathf.Rad2Deg * angle, Vector3.up);
				units.unitModel.transform.rotation = rotation;
			}

			selectedUnit.setStandardTexture();
			selectedUnit.setRange("off");
			targetTile.setTexture(Tile.tileTextureB);
			
			selectedUnit = null;
		}
	}

	private void resetUnitSelection(){
		selectedUnit.setStandardTexture();
		selectedUnit.setRange("off");
		selectedUnit = null;
		unitSelected = false;
	}

	private void setUnitSelection(Units unit){
		unitSelected = true;
		selectedUnit = unit;
		selectedUnit.setSelectedTexture();
		selectedUnit.setRange("on");
	}
	
	void Update(){

		if (resetSelection == true && selectedUnit != null) {
			resetUnitSelection();
		}
		resetSelection = false;
		
		ray = Camera.main.ScreenPointToRay (Input.mousePosition);

		if (Input.GetMouseButtonDown (0) && Physics.Raycast(ray, out hit)) {

			if ((hit.collider.name == "unit" || hit.collider.gameObject.name == "unitTile") && unitSelection == true){

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
										print("selecting n stuff");
									}
									else if (unit.playerIndex != Player.playerIndex && TouchHandler.unitAttacking == true){ // if the unit is on the other team, then set it to the units target
										selectedUnit.attackTarget = unit;
										unit.setSelectedTexture();

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
				
				float deltaZ = unit.currentTile.getZ() - unitPos.z;
				float deltaX = unit.currentTile.getX() - unitPos.x;
				float angle = (Mathf.Atan2(deltaZ, deltaX));
				
				float moveX = (float) Mathf.Cos((float)angle);
				float moveZ = (float) Mathf.Sin((float)angle);

				if (Mathf.Abs(unitPos.x - unit.currentTile.getX()) <= Mathf.Abs(moveX/Units.speed) &&
				    Mathf.Abs(unitPos.z - unit.currentTile.getZ()) <= Mathf.Abs(moveZ/Units.speed)){

					unit.transform.position = new Vector3(unit.currentTile.getX(), unitPos.y, unit.currentTile.getZ());

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

/*private Camera cam = new Camera();  && hit.collider.gameObject.name == "unit"
	private Player currentPlayer;
	private Grid grid;
	private GameObject selectedGeo;
	private Ray ray;
	private List<Units> movingUnits;
	private Quaternion rotation = new Quaternion();
	public Units selectedUnit;
	public bool unitSelection = false, unitMovement = false;
	public static bool moveUpdate;
	
	public Mousepicking(Camera camera, Player currentPlayer, Grid grid){
	}
	
	public void initialize(){
		inputManager.addMapping("clicked", new MouseButtonTrigger(MouseInput.BUTTON_LEFT));
		inputManager.addListener(actionListener,"clicked");
	}
	
	public void moveInitializing(Tile tile){
		if (tile.currentUnit == null){
			
			selectedUnit.currentTile.currentUnit = null;
			selectedUnit.currentTile = null;
			tile.currentUnit = selectedUnit;
			selectedUnit.currentTile = tile;
			selectedUnit.targetTile = tile;
			movingUnits.add(selectedUnit);
			
			for(Units unit : movingUnits){
				Vector3f unitPos = unit.getGeometry().getLocalTranslation();
				float deltaZ = unit.currentTile.getZ() - unitPos.z;
				float deltaX = unit.currentTile.getX() - unitPos.x;
				double angle = Math.atan2(deltaX, deltaZ);
				rotation.fromAngleAxis((float) angle, new Vector3f(0,1,0));
				unit.getGeometry().setLocalRotation(rotation);
			}
			
			selectedUnit.setStandardTexture();
			selectedUnit.setRange("off");
			tile.setTexture(Tile.tileTextureB);
			
			selectedUnit = null;
		}
	}
	
	public void updateUnits(){ // called by simpleUpdate if moveUpdate = true
		for(Units unit : movingUnits){
			Vector3f unitPos = unit.getGeometry().getLocalTranslation();
			
			float deltaZ =  unit.currentTile.getZ() - unitPos.z;
			float deltaX =  unit.currentTile.getX() - unitPos.x;
			double angle = Math.toDegrees(Math.atan2(deltaZ, deltaX));
			
			float moveX = (float) Math.cos(Math.toRadians(angle));
			float moveZ = (float) Math.sin(Math.toRadians(angle));
			
			if (Math.abs(unitPos.x - unit.currentTile.getX()) <= Math.abs(moveX/Units.speed) &&
			    Math.abs(unitPos.z - unit.currentTile.getZ()) <= Math.abs(moveZ/Units.speed)){
				unit.getGeometry().setLocalTranslation
					(unit.currentTile.getX(), unitPos.y, unit.currentTile.getZ());
				if (unit.currentTile.getTexture().getName() != Tile.tileTextureD){
					unit.currentTile.setTexture(Tile.tileTextureA);
				}
				movingUnits.remove(unit);
				break;
			}
			
			unit.getGeometry().setLocalTranslation
				(unitPos.x+moveX/Units.speed, 
				 unitPos.y, 
				 unitPos.z+moveZ/Units.speed);
		}
	}
	
	ActionListener actionListener = new ActionListener(){
		@Override
		public void onAction(String name, boolean keyPressed, float tpf) {
			if(name.equals("clicked") && !keyPressed){
				CollisionResults collisionResults = new CollisionResults();
				
				Vector2f click2d   = inputManager.getCursorPosition();
				Vector3f click3d   = cam.getWorldCoordinates(new Vector2f(click2d.x,click2d.y), 0).clone();
				Vector3f direction = cam.getWorldCoordinates(new Vector2f(click2d.x,click2d.y+0.1f), 1).subtractLocal(click3d).normalizeLocal();
				
				ray = new Ray(click3d, direction);
				
				rootNode.collideWith(ray, collisionResults);
				
				if (collisionResults.size() > 0){
					selectedGeo = collisionResults.getClosestCollision().getGeometry();
					
					if (unitSelection){
						for(Units[] i: currentPlayer.units){ // Finding the unit index from mouse picking the geometry
							for(Units unit : i){
								if (unit != null && selectedGeo != null){
									if (unit.getGeometry() == selectedGeo && movingUnits.contains(unit) == false){
										if (selectedUnit != null){
											selectedUnit.setStandardTexture(); // resetting texture to unselected units
											selectedUnit.selected = false;
										}
										selectedUnit = unit;
										selectedUnit.selected = true;
										selectedUnit.setRange("on");
									}
								}
							}
						}	
						if (selectedUnit != null){
							selectedUnit.setSelectedTexture(); 
						}
					}	
					
					if (unitMovement && selectedUnit != null){ // clicking the tile while having a unit selected
						for(Tile[] i: grid.getGrid()){ // Finding the unit index from mouse picking the geometry
							for(Tile tile : i){
								if (tile != null && selectedGeo != null && tile.currentUnit == null){
									if (tile.getGeometry() == selectedGeo){
										moveInitializing(tile);
									}
								}
							}
						}
					}
					
					if (movingUnits.isEmpty()){
						moveUpdate = false;
					} else {
						moveUpdate = true;
					}
					
				}
			}
			
		}
	};*/
