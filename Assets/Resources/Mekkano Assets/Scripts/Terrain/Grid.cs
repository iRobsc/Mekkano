using UnityEngine;
using System.Collections;

public class Grid : MonoBehaviour {

	private Tile[,] grid;
	private int w, l;
	public float height;

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
	
	public Tile getGrid(int x, int z){
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
