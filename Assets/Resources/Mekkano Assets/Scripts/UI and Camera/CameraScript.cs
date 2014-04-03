using UnityEngine;
using System.Collections;

public class CameraScript : MonoBehaviour
{

	GuiScript guiScript;
	//private int speed = 30;
	public int player;
	//private Transform curPos;
	private Vector3 pos, rot;

	
	
	// Use this for initialization
	void Start () {
		/*
		GameObject tempObj = Instantiate(Resources.Load("guiScript")) as GameObject;
		guiScript = tempObj.GetComponent<GuiScript>();
		*/
		
	}
	
	// Update is called once per frame
	void Update () {
		player = Player.playerIndex;
		pos = new Vector3(-5, 30, 11);
		rot = new Vector3(60, 90, 0);

		pos.x = (player==1?-5:40);
		rot.y = (player==1?90:270);
		rot.z = (player==1?0:180);

		Camera.main.transform.position = Vector3.Lerp(Camera.main.transform.position, pos, Time.deltaTime * 2);
		Camera.main.transform.rotation = Quaternion.Lerp(Camera.main.transform.rotation, Quaternion.Euler(rot), Time.deltaTime * 2);
	}
}
