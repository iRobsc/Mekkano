using UnityEngine;
using System.Collections;

public class Units : MonoBehaviour {
	
	public int hp;
	public bool selected;
	public Tile currentTile, targetTile;
	public double angle;
	public static float speed = 10; // closer to 0 equals faster (10 is a good speed)
	protected int tileRange, movementPoints;
	protected float scale, x, z, damage;
	private float gridHeight;
	private Texture unitStandardTexture, unitSelectedTexture;
	private Grid grid;
	private Texture standardTexture;
	private Tile[,] unitRange;
	private Vector3 rotation;

	private GameObject attackLine;

	public GameObject unitModel;
	public int moveRange, attackRange;
	public int playerIndex;
	public Units attackTarget;
	
	public virtual void create(Tile tile, bool side) {/* polymorphic method*/}
	
	protected void createUnit(string texturePath, string model, Tile tile, Vector3 scaling, bool side, int moveRange, int attackRange){
		grid = Main.grid;
		gridHeight = grid.height;
		standardTexture = (Texture2D)Resources.Load (texturePath);
		unitModel = Instantiate(Resources.Load(model)) as GameObject;
		unitModel.gameObject.GetComponentInChildren<MeshRenderer> ().material.mainTexture = standardTexture;
		Transform obj = unitModel.transform.GetChild(0); 
		obj.gameObject.AddComponent<MeshCollider> ();
		unitModel.transform.GetChild(0).name = "unit";

		unitModel.transform.position = new Vector3 (tile.getXindex(), gridHeight , tile.getZindex());
		unitModel.transform.localScale = scaling;
		unitModel.transform.rotation = Quaternion.AngleAxis (side?+90:-90, Vector3.up);

		this.moveRange = moveRange;
		this.attackRange = attackRange;
	}

	public void attack(Tile tile){
		
	}

	public void targetLine(Tile currentTile, Tile targetTile){

		float deltaX = currentTile.getXpos()-targetTile.getXpos();
		float deltaZ = currentTile.getZpos()-targetTile.getZpos();

		float distance = Mathf.Sqrt(Mathf.Pow(deltaX,2) + 
		                            Mathf.Pow(deltaZ,2));

		if (attackLine == null)	attackLine = GameObject.CreatePrimitive(PrimitiveType.Quad);

		attackLine.transform.position = new Vector3 (currentTile.getXpos()-deltaX/2,
		                                                  currentTile.getYpos()+0.1f, 
														  currentTile.getZpos()-deltaZ/2);

		attackLine.transform.localScale = new Vector3(0.2f,distance,0);
		attackLine.transform.localRotation = Quaternion.Euler(new Vector3 
		                                    (90, TouchHandler.calculateAngle
		 									(currentTile.getXpos(), 
		                                     currentTile.getZpos(), 
		                                     targetTile.getXpos(), 
		                                     targetTile.getZpos()) * Mathf.Rad2Deg, 0));
	}

	public void removeLine(){
		Destroy(attackLine);
	}

	public void faceTarget(Units unit, Tile targetTile){
		Vector3 unitPos = unit.unitModel.transform.position;
		unit.unitModel.transform.localRotation = Quaternion.Euler(new Vector3 
			 									(0, TouchHandler.calculateAngle(
												 unit.unitModel.transform.position.x, 
											   	 unit.unitModel.transform.position.z, 
		                                         targetTile.getXpos(), 
		                                         targetTile.getZpos()) * Mathf.Rad2Deg, 0));
	}

	public void faceTarget(Units unit, Units targetUnit){
		faceTarget(unit, targetUnit.currentTile);
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
								unitRange[x,z].setTexture(Tile.tileTextureB);
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
