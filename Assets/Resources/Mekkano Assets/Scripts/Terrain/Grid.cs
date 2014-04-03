using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Linq;


public class Grid : MonoBehaviour {

	private Tile[,] grid;
	private Tile[,] rangeTiles;
	private List<Tile> buffTiles = new List<Tile>();
	private int w, l;
	private float height;

	public void createGrid(float xPos, float zPos, int width, int length, float gridHeight){
		grid = new Tile[width,length];
		w = width;
		l = length;
		height = gridHeight;

		for(int X = 0; X < width; X++){
			for(int Z = 0; Z < length; Z++){
				grid[X,Z] = gameObject.AddComponent<Tile>();
				grid[X,Z].createTile(X*Tile.width + xPos, height, Z*Tile.length + zPos);
				grid[X,Z].setCoords(X,Z);
			}
		}
	}

	private bool tileInRange(int x, int y, int range){
		if (range == 1) return true;
		else if (range < 4) return (Mathf.Abs(x) + Mathf.Abs(y) <= range);
		else return (Mathf.Abs(Mathf.Pow(x,2)) + Mathf.Abs(Mathf.Pow(y,2)) <= Mathf.Pow(range,2));
	}

	private void checkBuffTiles(Tile tile, bool leavingTile){
		foreach(Units buffSource in tile.buffSources){
			if (tile.currentUnit != null){
				if (tile.currentUnit.hasBuff && leavingTile){
					buffSource.buff(tile.currentUnit, true);
				} else if (tile.currentUnit.hasBuff == false && leavingTile == false){
					buffSource.buff(tile.currentUnit, false);
				}
			}
		}
	}

	public void setRange(Tile centerPoint, int range, bool buffTiles){
		int xyLength = range+(range+1);
		
		rangeTiles = new Tile[xyLength, xyLength];

		if(buffTiles == true){
			if (centerPoint.currentUnit.buffTiles != null){
				foreach(Tile tile in centerPoint.currentUnit.buffTiles){
					if (tile != null) {
						checkBuffTiles(tile, true);
						tile.buffSources.Remove(centerPoint.currentUnit);
					}
				}
			}
			centerPoint.currentUnit.buffTiles = null;
			centerPoint.currentUnit.buffTiles = rangeTiles;
		}
		else centerPoint.currentUnit.rangeTiles = rangeTiles;
		
		for(int x = 0; x < xyLength; x++){
			for(int y = 0; y < xyLength; y++){
				if(tileInRange((0-range)+x, (0-range)+y, range)){
					if((centerPoint.xCoord-range)+x >= 0 && 
					   (centerPoint.xCoord-range)+x < Main.gridXlength &&
					   (centerPoint.zCoord-range)+y >= 0 &&
					   (centerPoint.zCoord-range)+y < Main.gridZlength){
						rangeTiles[x,y] = grid[centerPoint.xCoord-range+x, centerPoint.zCoord-range+y];
						if (buffTiles == true) {
							rangeTiles[x,y].buffSources.Add(centerPoint.currentUnit);
							checkBuffTiles(rangeTiles[x,y], false);
						}
					}
				}
			}
		}
	}
	
	public void showRange(Units selectedUnit){
		Tile[,] rangeTiles = (selectedUnit.rangeTiles != null? 
		                      selectedUnit.rangeTiles : selectedUnit.buffTiles);
		if(selectedUnit.buffTiles != null) buffTiles.AddRange(selectedUnit.buffTiles.Cast<Tile>().ToList());

		int center = (int)Mathf.Ceil(rangeTiles.GetLength(0)/2);
		Tile centerPoint = rangeTiles[center, center];

		for(int x = 0; x < rangeTiles.GetLength(0); x++){
			for (int z = 0; z < rangeTiles.GetLength(0); z++){
				if (rangeTiles[x,z] != null){
					if (rangeTiles[x,z].tileMesh.name == "tile"){
						rangeTiles[x,z].setTexture(Tile.tileTextureC); // open tile texture
					} else {
						rangeTiles[x,z].setTexture(Tile.tileTextureD); // closed tile texture
						if (TouchHandler.unitAttacking && rangeTiles[x,z].currentUnit != null 
						    && selectedUnit.playerIndex != 0){
						    if (rangeTiles[x,z].currentUnit.playerIndex != centerPoint.currentUnit.playerIndex &&
							    rangeTiles[x,z].currentUnit.playerIndex != 0){

								rangeTiles[x,z].setTexture(Tile.tileTextureE);
							}
						} if(selectedUnit.aura == true && 
						         (selectedUnit.playerIndex == rangeTiles[x,z].currentUnit.playerIndex ||
						     	  selectedUnit.playerIndex == 0) &&
						          buffTiles.ToList().Contains(rangeTiles[x,z])){

							rangeTiles[x,z].setTexture(Tile.tileTextureF);
						}
					}
				}
			}
		} 
		buffTiles.Clear();
	}

	public void hideRange(Tile[,] rangeTiles){
		foreach(Tile tile in rangeTiles){
			if(tile != null) tile.setTexture(Tile.tileTextureA);
		}
	}

	/*public void checkAura(Tile[,] rangeTiles, bool conquest, Units auraSource){
		int player1Counter = 0, player2Counter = 0;
		foreach(Tile auraTile in rangeTiles) {
			if(auraTile.currentUnit != null) {
				if(conquest) {
					if (auraTile.currentUnit.playerIndex == 1) player1Counter += 1; 
					else if (auraTile.currentUnit.playerIndex == 2) player2Counter += 1;
				} else auraSource.giveBuff();
			}
		}
	}*/
	
	public Tile getTile(int x, int z){
		return grid[x,z];
	}
	
	public Tile[,] getGrid(){
		return grid;
	}
	
	public void addTile(int x, int z, Tile[,] fullGrid){
		grid[x,z] = fullGrid[x,z];
	}
	
	public int getGridLength(){
		return l;
	}
	
	public int getGridWidth(){
		return w;
	}

}
