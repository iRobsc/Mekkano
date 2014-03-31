using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class Tile : MonoBehaviour{
	
	private float x, z;
	public List<Units> buffSources = new List<Units>();
	public int xCoord, zCoord;
	public static float width = 2.5f, length = 2.5f;
	public Units currentUnit = null;
	public Texture texture;
	private Texture currentTexture;
	public static string tileTextureA = "Mekkano Assets/Textures/tileA", 
						 tileTextureB = "Mekkano Assets/Textures/tileB", 
						 tileTextureC = "Mekkano Assets/Textures/tileC",
						 tileTextureD = "Mekkano Assets/Textures/tileD",
						 tileTextureE = "Mekkano Assets/Textures/tileE",
						 tileTextureF = "Mekkano Assets/Textures/tileF";
	private Material spriteMaterial;
	public GameObject tileMesh;

	public float getXindex(){
		return x;
	}
	
	public float getZindex(){
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

	public float getXpos(){
		return tileMesh.transform.localPosition.x;
	}

	public float getYpos(){
		return tileMesh.transform.localPosition.y;
	}

	public float getZpos(){
		return tileMesh.transform.localPosition.z;
	}
	
	public void createTile(float x, float y, float z){

		tileMesh = GameObject.CreatePrimitive (PrimitiveType.Quad);
		setTexture (tileTextureA);
		tileMesh.renderer.material.shader = Shader.Find("Unlit/Transparent");
		tileMesh.layer = LayerMask.NameToLayer("Tiles");
		tileMesh.name = "tile";

		tileMesh.transform.localScale = new Vector3 (width, length, 1);
		tileMesh.transform.localPosition = new Vector3 (x, y, z);
		tileMesh.transform.Rotate (new Vector3 (90,0,0));

		this.x = x;
		this.z = z;
	}

	public void setTexture(string texturePath){
		texture = (Texture2D)Resources.Load(texturePath);
		tileMesh.renderer.material.mainTexture = texture;
	}

}
