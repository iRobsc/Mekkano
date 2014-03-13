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
	public GameObject unitModel;
	private Texture unitStandardTexture, unitSelectedTexture;
	private Grid grid;
	private Texture standardTexture;
	public Tile[,] unitRange;
	private Vector3 rotation;
	
	public virtual void create(Tile tile, bool side) {/* polymorphic method*/}
	
	protected void createUnit(string texturePath, string model, Tile tile, Vector3 scaling, bool side){
		grid = Main.grid;
		gridHeight = grid.height;
		standardTexture = (Texture2D)Resources.Load (texturePath);
		unitModel = Instantiate(Resources.Load(model)) as GameObject;
		unitModel.gameObject.GetComponentInChildren<MeshRenderer> ().material.mainTexture = standardTexture;
		Transform obj = unitModel.transform.GetChild(0); 
		obj.gameObject.AddComponent<MeshCollider> ();
		unitModel.transform.GetChild(0).name = "unit";
		
		unitModel.transform.position = new Vector3 (tile.getX(), gridHeight , tile.getZ());
		unitModel.transform.localScale = scaling;
		unitModel.transform.rotation = Quaternion.AngleAxis (side?+90:-90, Vector3.up);
	}

	public void attack(Tile tile){
		
	}
	
	public void setRange(string onOff){
		if (onOff == "on"){
			unitRange = new Tile[3,3];
			unitRange[0,0] = grid.getGrid(currentTile.xCoord,currentTile.zCoord);
			unitRange[0,1] = grid.getGrid(currentTile.xCoord,currentTile.zCoord+1);
			unitRange[1,0] = grid.getGrid(currentTile.xCoord+1,currentTile.zCoord);
			unitRange[1,1] = grid.getGrid(currentTile.xCoord+1,currentTile.zCoord+1);

			for(int x = 0; x < 2; x++){
				for (int z = 0; z < 2; z++)
				if (unitRange[x,z].currentUnit == null){
					unitRange[x,z].setTexture(Tile.tileTextureC);
				} else {
					unitRange[x,z].setTexture(Tile.tileTextureD);
				}
			}
		} else if (onOff == "off"){
			unitRange = grid.getGrid();
			for(int x = 0; x < grid.getGridWidth(); x++){
				for (int z = 0; z < grid.getGridLength(); z++)
					unitRange[x,z].setTexture(Tile.tileTextureA);
			}
		}
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
