using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class Units : MonoBehaviour {

	protected int tileRange, movementPoints;
	protected float scale, x, z;

	public bool selected;
	public Tile currentTile, targetTile;
	public double angle;
	public static float speed = 10; // closer to 0 equals faster (10 is a good speed)
	private float gridHeight;
	private Texture unitStandardTexture, unitSelectedTexture;
	private Grid grid;
	private Texture standardTexture, attackTexture;
	private Vector3 rotation;

	public GameObject attackLine;
	public static List<GameObject> lines = new List<GameObject> ();
	public Tile[,] rangeTiles;

	public GameObject unitModel;
	public int playerIndex;
	public Units attackTarget;

	public int damage, buffDamage;

	// variables from the subclass
	protected Vector3 scaling;
	protected string model, texture;
	public bool moveable, aura;
	public int moveRange, attackRange, range, baseDamage, hp;
	
	public virtual void create(Tile tile, bool side) {/* polymorphic method*/}
	public virtual void create(Tile tile) {/* polymorphic method*/}

	void Start(){
		grid = Main.grid;
		gridHeight = grid.height;
	}

	protected void createUnit(Tile tile, bool side){
		createModel(tile);
		unitModel.transform.position = new Vector3 (tile.getXindex(), gridHeight , tile.getZindex());
		unitModel.transform.localScale = scaling;
		unitModel.transform.rotation = Quaternion.AngleAxis (side?+90:-90, Vector3.up);
		playerIndex = (side?1:2);
		calculateDamage();
	}

	protected void createObject(Tile tile){
		createModel(tile);
		unitModel.transform.position = new Vector3 (tile.getXindex(), gridHeight , tile.getZindex());
		unitModel.transform.localScale = scaling;
	}

	private void createModel(Tile tile){
		standardTexture = (Texture2D)Resources.Load(texture);
		unitModel = Instantiate(Resources.Load(model)) as GameObject;
		unitModel.gameObject.GetComponentInChildren<MeshRenderer> ().material.mainTexture = standardTexture;
		
		Transform obj = unitModel.transform.GetChild(0); 
		obj.gameObject.AddComponent<MeshCollider> ();
		unitModel.transform.GetChild(0).name = "object";

		currentTile = tile;
		currentTile.currentUnit = this;

		tile.tile.name = "collisionTile";
	}

	public void calculateDamage(){
		damage = baseDamage + buffDamage;
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
		attacker.attackTarget = null;
	}

	public void die(Units target){
		//play death animation
		target.currentTile.tile.name = "tile";
		Main.allUnits.Remove (target);
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
					targetUnit.attackLine.renderer.material.color = new Color(0.8f,0.8f,0.8f,1);
					attackTexture = (Texture2D)Resources.Load("Mekkano Assets/Textures/attackCollide");
					targetUnit.attackLine.renderer.material.mainTexture = attackTexture;
				} else {
					attackTexture = (Texture2D)Resources.Load("Mekkano Assets/Textures/p1Attack");
					lineToChange.renderer.material.mainTexture = attackTexture;
				}

			} else{
				if (atkEachother == true){
					targetUnit.attackLine.renderer.material.color = new Color(0.8f,0.8f,0.8f,1);
					attackTexture = (Texture2D)Resources.Load("Mekkano Assets/Textures/attackCollide");
					targetUnit.attackLine.renderer.material.mainTexture = attackTexture;
					targetUnit.attackLine.renderer.material.SetTextureScale("_MainTex", new Vector2(-1,-1));
				}
				else {
					attackTexture = (Texture2D)Resources.Load("Mekkano Assets/Textures/p2Attack");
					lineToChange.renderer.material.mainTexture = attackTexture;
				}
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

	public bool inRange(Tile tile){
		foreach (Tile currentTile in rangeTiles) {
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
	
	public void setStandardTexture(){
		unitModel.gameObject.GetComponentInChildren<MeshRenderer> ().material.color = new Color(0.800f,0.800f,0.800f,1);
	}

	public void setSelectedTexture(){
		unitModel.gameObject.GetComponentInChildren<MeshRenderer> ().material.SetColor ("_Color", Color.green);
	}
	
}
