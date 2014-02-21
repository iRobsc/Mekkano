using UnityEngine;
using System.Collections;

public class Units : MonoBehaviour {

	public int hp;
	public bool selected;
	public Tile currentTile, targetTile;
	public double angle;
	public static float speed = 100; // closer to 0 equals faster (100 is a good speed)
	protected int tileRange, movementPoints;
	protected float scale, x, z, damage, gridHeight;
	private GameObject unitModel;
	private Texture unitStandardTexture, unitSelectedTexture;
	public Tile[,] unitRange;
	private Grid fullGrid;
	
	public void create(Grid grid, Tile tile, float gridHeight, bool side) {/* polymorphic method*/}
	
	protected void createUnit(string texturePath, string model, Grid grid, Tile tile, float scaling, float GridHeight, bool side){
		gridHeight = GridHeight;
		fullGrid = grid;
		unitModel = Resources.Load ("Mekkano Assets/Models/RangedUnit");
		Texture texture = (Texture2D)Resources.Load(texturePath);
		unitModel.renderer.material.mainTexture = texture;
		unitModel.name = "unit";

		unitModel.transform.position = new Vector3 (tile.getX(), gridHeight , tile.getZ());
		unitModel.transform.localScale = scaling;
		unitModel.transform.Rotate (Vector3 (0,side?+90:-90,0));
	}
	
	public void moveToTile(Units unit, Tile targetTile){
		if (unit != null){
			/*Mousepicking.moveUpdate = true;
			Phases.mousePicking.selectedUnit = unit;
			Phases.mousePicking.moveInitializing(targetTile);*/
		}
	}
	
	public void attack(Tile tile){
		
	}
	
	public void setRange(string onOff){
		if (onOff == "on"){
			unitRange = new Tile[3,3];
			unitRange[0,0] = fullGrid.getGrid(currentTile.xCoord,currentTile.zCoord);
			unitRange[0,1] = fullGrid.getGrid(currentTile.xCoord,currentTile.zCoord+1);
			unitRange[1,0] = fullGrid.getGrid(currentTile.xCoord+1,currentTile.zCoord);
			unitRange[1,1] = fullGrid.getGrid(currentTile.xCoord+1,currentTile.zCoord+1);
			
			
			for(int x = 0; x < 2; x++){
				for (int z = 0; z < 2; z++)
				if (unitRange[x,z].currentUnit == null){
					unitRange[x,z].setTexture(Tile.tileTextureC);
				} else {
					unitRange[x,z].setTexture(Tile.tileTextureD);
				}
			}
		} else if (onOff == "off"){
			unitRange = fullGrid.getGrid();
			for(int x = 0; x < fullGrid.getGridWidth(); x++){
				for (int z = 0; z < fullGrid.getGridLength(); z++)
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
	
	/*public Geometry getGeometry() {
		Geometry geo = (Geometry) unitModel;
		return geo;
	}
	
	public void setStandardTexture(){
		((Geometry) unitModel).getMaterial().setTexture("DiffuseMap", unitStandardTexture);
	}
	
	public void setSelectedTexture(){
		((Geometry) unitModel).getMaterial().setTexture("DiffuseMap", unitSelectedTexture);
	}*/

}
