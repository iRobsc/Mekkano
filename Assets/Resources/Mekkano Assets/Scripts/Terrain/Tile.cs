using UnityEngine;
using System.Collections;

public class Tile : MonoBehaviour{
	
	private float x, z;
	public int xCoord, zCoord;
	public static float width = 2, length = 2;
	public Units currentUnit = null;
	public Texture texture;
	public static string tileTextureA = "Mekkano Assets/Textures/tileA", tileTextureB, tileTextureC,tileTextureD = "Mekkano Assets/Textures/tileB";
	private Material spriteMaterial;
	public GameObject tile;

	public float getX(){
		return x;
	}
	
	public float getZ(){
		return z;
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

	/*public void setTexture(String textureName){
		texture = assetManager.loadTexture(textureName);
		spriteMaterial = new Material(assetManager, "Common/MatDefs/Misc/Unshaded.j3md");
		spriteMaterial.setTexture("ColorMap", getTexture());
		getTexture().setMagFilter(Texture.MagFilter.Nearest);
		spriteMaterial.getAdditionalRenderState().setBlendMode(BlendMode.Alpha);
		tile.setMaterial(spriteMaterial);
	}*/
}
