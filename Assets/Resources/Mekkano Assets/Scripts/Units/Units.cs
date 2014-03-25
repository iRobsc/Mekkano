using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class Units : MonoBehaviour {

	public bool selected;
	public Tile currentTile, targetTile;
	public double angle;
	public static float speed = 10; // closer to 0 equals faster (10 is a good speed)
	protected int tileRange, movementPoints;
	protected float scale, x, z;
	private float gridHeight;
	private Texture unitStandardTexture, unitSelectedTexture;
	private Grid grid;
	private Texture standardTexture;
	private Tile[,] unitRange;
	private Vector3 rotation;
	public GameObject attackLine;
	public static List<GameObject> lines = new List<GameObject> ();

	public GameObject unitModel;
	public int playerIndex;
	public Units attackTarget;

	// variables from the subclass
	protected Vector3 scaling;
	protected string model, texture;
	public int moveRange, attackRange, damage, hp;
	
	public virtual void create(Tile tile, bool side) {/* polymorphic method*/}

	protected void createUnit(Tile tile, bool side){
		grid = Main.grid;
		gridHeight = grid.height;
		standardTexture = (Texture2D)Resources.Load (texture);
		unitModel = Instantiate(Resources.Load(model)) as GameObject;
		unitModel.gameObject.GetComponentInChildren<MeshRenderer> ().material.mainTexture = standardTexture;
		Transform obj = unitModel.transform.GetChild(0); 
		obj.gameObject.AddComponent<MeshCollider> ();
		unitModel.transform.GetChild(0).name = "unit";

		unitModel.transform.position = new Vector3 (tile.getXindex(), gridHeight , tile.getZindex());
		unitModel.transform.localScale = scaling;
		unitModel.transform.rotation = Quaternion.AngleAxis (side?+90:-90, Vector3.up);
	}

	public static void engageAttacks(List<Units> Units){
		List<Units> attackList = new List<Units>();

		foreach (Units unit in Units) {
			if (unit != null){
				if(unit.attackTarget != null){
					attackList.Add(unit);
				}
			}
		}

		foreach (Units unit in attackList) {
			unit.attack(unit, unit.attackTarget);
		}

		foreach (GameObject line in lines) {
			Destroy(line);
		}
		lines.Clear ();
	}

	public void attack(Units attacker, Units target){
		//play attack animation
		target.hp -= attacker.damage;
		if (target.hp <= 0) target.die(target);
	}

	public void die(Units target){
		//play death animation
		target.currentTile.tile.name = "tile";
		Main.bothPlayerUnits.Remove (target);
		Destroy(target.unitModel);
		Destroy(target);
	}

	public void targetLine(Tile currentTile, Tile targetTile, bool atkEachother){
	
		Units targetUnit = currentTile.currentUnit.attackTarget;

		float deltaX = currentTile.getXpos()-targetTile.getXpos();
		float deltaZ = currentTile.getZpos()-targetTile.getZpos();

		float distance = Mathf.Sqrt(Mathf.Pow(deltaX,2) + 
		                            Mathf.Pow(deltaZ,2));
		if (targetUnit != null){
			if (currentTile.currentUnit == targetUnit.attackTarget){
				atkEachother = false;
				attackLine = GameObject.CreatePrimitive(PrimitiveType.Quad);
				setLineMaterial(atkEachother, currentTile.currentUnit, targetUnit.attackLine);
			}
		}

		currentTile.currentUnit.attackTarget = targetTile.currentUnit;

		if(currentTile.currentUnit.attackTarget == targetTile.currentUnit && 
		   targetTile.currentUnit.attackTarget == currentTile.currentUnit) atkEachother = true;		

		if (attackLine == null) attackLine = GameObject.CreatePrimitive(PrimitiveType.Quad);

		if (atkEachother == false) {
			lines.Add (attackLine);
			attackLine.transform.position = new Vector3(currentTile.getXpos()-deltaX/2,
		                                                currentTile.getYpos()+1, 
														currentTile.getZpos()-deltaZ/2);

			attackLine.transform.localScale = new Vector3(0.2f,distance,0);
			attackLine.transform.localRotation = Quaternion.Euler(new Vector3 
		                                    (90, TouchHandler.calculateAngle
		 									(currentTile.getXpos(), 
		                                     currentTile.getZpos(), 
		                                     targetTile.getXpos(), 
		                                     targetTile.getZpos()) * Mathf.Rad2Deg, 0));

			attackLine.renderer.material.shader = Shader.Find ("Mobile/Vertex Colored");
		}
		else {
			Destroy(attackLine);
		}
		setLineMaterial(atkEachother, targetTile.currentUnit, attackLine);
	}

	private void setLineMaterial(bool atkEachother, Units targetUnit, GameObject lineToChange){

			if (targetUnit.playerIndex == 1){
				if (atkEachother == true){
					targetUnit.attackLine.renderer.material.color = new Color(0.800f,0.800f,0.800f,1);
					Texture collideTexture = (Texture2D)Resources.Load("Mekkano Assets/Textures/attackCollide");
					targetUnit.attackLine.renderer.material.mainTexture = collideTexture;
				} else lineToChange.renderer.material.color = new Color (255 / 255, 123 / 255, 123 / 255);

			} else{
				if (atkEachother == true){
					targetUnit.attackLine.renderer.material.color = new Color(0.800f,0.800f,0.800f,1);
					Texture collideTexture = (Texture2D)Resources.Load("Mekkano Assets/Textures/attackCollide");
					targetUnit.attackLine.renderer.material.mainTexture = collideTexture;
					targetUnit.attackLine.renderer.material.SetTextureScale("_MainTex", new Vector2(-1,-1));
				}
				else lineToChange.renderer.material.color = new Color (81 / 255, 71 / 255, 255 / 255);
			}
	}

	public void faceTarget(Units unit, Tile targetTile){
		Vector3 unitPos = unit.unitModel.transform.position;
		unit.unitModel.transform.localRotation = Quaternion.Euler(new Vector3 
			 									(0, TouchHandler.calculateAngle(
												 unitPos.x, 
											   	 unitPos.z, 
		                                         targetTile.getXpos(), 
		                                         targetTile.getZpos()) * Mathf.Rad2Deg, 0));
	}

	public void setRange(bool removeRange, bool moveOrAttack){
		int range;
		if (moveOrAttack == true) range = moveRange;
		else range = attackRange;
		if (removeRange == true){
			int xyLength = range+(range+1);
			unitRange = new Tile[xyLength, xyLength];
			for(int x = 0; x < xyLength; x++){
				for(int y = 0; y < xyLength; y++){
					if(tileInRange((0-range)+x, (0-range)+y, range)){
						if ((currentTile.xCoord-range)+x >= 0 && 
						    (currentTile.xCoord-range)+x < Main.gridXlength &&
						    (currentTile.zCoord-range)+y >= 0 &&
						    (currentTile.zCoord-range)+y < Main.gridZlength){
							unitRange[x,y] = grid.getGrid(((currentTile.xCoord-range)+x),((currentTile.zCoord-range)+y));
						}
					}
				}
			}

			for(int x = 0; x < xyLength; x++){
				for (int z = 0; z < xyLength; z++){
					if (unitRange[x,z] != null){
						if (unitRange[x,z].currentUnit == null){
							unitRange[x,z].setTexture(Tile.tileTextureC);
						} else {
							if (TouchHandler.unitSelection){
								unitRange[x,z].setTexture(Tile.tileTextureD);
							} if (TouchHandler.unitAttacking && unitRange[x,z].currentUnit.playerIndex != playerIndex){
								unitRange[x,z].setTexture(Tile.tileTextureE);
							}
						}
					}
				}
			}
		} else if (removeRange == false){
				unitRange = grid.getGrid();
				for(int x = 0; x < grid.getGridWidth(); x++){
					for (int z = 0; z < grid.getGridLength(); z++)
						unitRange[x,z].setTexture(Tile.tileTextureA);
					}
			}
	}

	private bool tileInRange(int x, int y, int range){
		if (range == 1) return true;
		else if (range < 4) return (Mathf.Abs(x) + Mathf.Abs(y) <= range);
		else return (Mathf.Abs(Mathf.Pow(x,2)) + Mathf.Abs(Mathf.Pow(y,2)) <= Mathf.Pow(range,2));
	}

	public bool inRange(Tile tile){
		foreach (Tile currentTile in unitRange) {
			if (currentTile == tile) return true;
		}
		return false;
	}

	public int getTileRange(){
		return tileRange;
	}
	
	public int getMovementPoints(){
		return movementPoints;
	}
	
	public float getScale(){
		return scale;
	}
	
	public float getX(){
		return x;
	}
	
	public float getZ(){
		return z;
	}
	
	public float getDamage(){
		return damage;
	}
	
	public void setStandardTexture(){
		unitModel.gameObject.GetComponentInChildren<MeshRenderer> ().material.color = new Color(0.800f,0.800f,0.800f,1);
	}

	public void setSelectedTexture(){
		unitModel.gameObject.GetComponentInChildren<MeshRenderer> ().material.SetColor ("_Color", Color.green);
	}
	
}
