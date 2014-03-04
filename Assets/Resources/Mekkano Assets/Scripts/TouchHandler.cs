using UnityEngine;
using System.Collections;

public class TouchHandler : MonoBehaviour {

	private Ray ray;
	private RaycastHit hit;
	private bool unitSelected = false;
	private Units selectedUnit;
	private Player currentPlayer;

	void Start(){
		currentPlayer = Main.player1;
	}

	void Update(){
		ray = Camera.main.ScreenPointToRay (Input.mousePosition);

		if (Input.GetMouseButtonDown (0) && Physics.Raycast(ray, out hit)) {

			if (hit.collider.name == "unit"){

				foreach(Units unit in currentPlayer.units){
					if (unit != null){
						if(unit.unitModel.transform.GetChild(0) == hit.collider.transform){ // searching for the selected geometry in the currentUnit's array
								if (selectedUnit != null){
									selectedUnit.setStandardTexture(); // resetting texture to unselected units
									selectedUnit.selected = false;
								}
								unitSelected = true;
								selectedUnit = unit;
								selectedUnit.selected = true;
								selectedUnit.setSelectedTexture();
								//selectedUnit.setRange("on");
							}

					}
				}

			}

			if (hit.collider.gameObject.name == "tile" && unitSelected == true){

				foreach(Tile tile in Main.grid){
					if (tile != null){
						if(tile == hit.collider.transform){ // searching for the selected geometry in the currentUnit's array
							if (tile.currentUnit == null){
								//units.Move(); -- make it happen plz
							}
						}
						
					}
				}

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
