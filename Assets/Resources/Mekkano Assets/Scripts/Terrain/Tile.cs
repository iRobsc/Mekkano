using UnityEngine;
using System.Collections;

public class Tile : MonoBehaviour{
	
	private float x, z;
	public int xCoord, zCoord;
	public static float width = 1.5f, length = 1.5f;
	public Units currentUnit = null;
	public Texture texture;
	public static string tileTextureA, tileTextureB, tileTextureC,tileTextureD;
	private Material spriteMaterial;
	//private Geometry tile;

	public float getX(){
		return x + Tile.width/2;
	}
	
	public float getZ(){
		return z - Tile.length/2;
	}
	/*
	public Texture getTexture(){
		return texture;
	}
	
	public Geometry getGeometry(){
		return tile;
	}*/
	
	public void setCoords(int X, int Z){
		xCoord = X;
		zCoord = Z;
	}
	
	public void createTile(float X, float Y ,float Z){
		/*Quad quadShape = new Quad(width,length);
		tile = new Geometry ("tile", quadShape);
		setTexture(tileTextureA);
		spriteMaterial = new Material(assetManager, "Common/MatDefs/Misc/Unshaded.j3md");
		spriteMaterial.setTexture("ColorMap", getTexture());
		spriteMaterial.getAdditionalRenderState().setBlendMode(BlendMode.Alpha);
		getTexture().setMagFilter(Texture.MagFilter.Nearest);
		
		tile.setLocalTranslation(X,Y,Z);
		
		x = X;
		z = Z;
		
		tile.rotate(-90*FastMath.DEG_TO_RAD, 0, 0);
		tile.setQueueBucket(Bucket.Transparent);
		tile.setMaterial(spriteMaterial);
		
		rootNode.attachChild(tile); */

		GameObject tile = GameObject.CreatePrimitive (PrimitiveType.Quad);
		setTexture (tile, "Mekkano Assets/Textures/Test textures/tileB");
		tile.name = "tile";

		x = X;
		z = Z;

		tile.transform.localPosition = new Vector3 (x, 0, z);
		tile.transform.Rotate (new Vector3 (90,0,0));
	}

	public void setTexture(GameObject tile, string texturePath){
		Texture texture = (Texture2D)Resources.Load(texturePath);
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
