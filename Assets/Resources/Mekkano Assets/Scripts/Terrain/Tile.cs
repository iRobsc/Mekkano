using UnityEngine;
using System.Collections;

public class Tile : MonoBehaviour{
	
	private float x, z;
	public int xCoord, zCoord;
	public static float width = 2.5f, length = 2.5f;
	public Units currentUnit = null;
	public Texture texture;
	private Texture currentTexture;
	public static string tileTextureA = "Mekkano Assets/Textures/tileA", tileTextureB = "Mekkano Assets/Textures/tileB", tileTextureC = "Mekkano Assets/Textures/tileC",tileTextureD = "Mekkano Assets/Textures/tileD";
	private Material spriteMaterial;
	public GameObject tile;

	public float getX(){
		return x;
	}
	
	public float getZ(){
		return z;
	}

	public Texture getTexture(string tileTexture){
		currentTexture = (Texture2D)Resources.Load (tileTexture);
		return currentTexture;
	}

	public Texture getTexture(){
		return texture;
	}
	
	public void setCoords(int X, int Z){
		xCoord = X;
		zCoord = Z;
	}
	
	public void createTile(float X, float Y ,float Z){

		tile = GameObject.CreatePrimitive (PrimitiveType.Quad);
		setTexture (tileTextureA);
		tile.renderer.material.shader = Shader.Find ("Unlit/Transparent");
		tile.name = "tile";

		x = X;
		z = Z;

		tile.transform.localScale = new Vector3 (width, length, 1);
		tile.transform.localPosition = new Vector3 (x, Y, z);
		tile.transform.Rotate (new Vector3 (90,0,0));
	}

	public void setTexture(string texturePath){
		texture = (Texture2D)Resources.Load(texturePath);
		tile.renderer.material.mainTexture = texture;
	}

}
