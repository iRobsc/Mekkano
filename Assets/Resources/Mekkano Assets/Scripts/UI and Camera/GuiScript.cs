using UnityEngine;
using System.Collections;

public class GuiScript : MonoBehaviour {




	private int dmg, hp;
	public string loadTexture;
	public TextMesh textMesh;
	public Texture2D button, button2, button3, phaseTexture, backTexture;
	private float rotAngle;
	private Vector2 pivotPoint;
	private int screenWScale = Screen.width/10;
	private int screenHScale = Screen.height/10;
	public int player;
	//public int phase = 1;
	private string buttonText = "End Phase";
	
	// Use this for initialization
	void Start () {

		//		content.image = (Texture2D)buttonImage;
		
	}
	
	// Update is called once per frame
	void Update () {


		
		//		textMesh.text = "Phase " + phase + "\n player " + player;
		
	}
	
	void OnGUI(){

		if (TouchHandler.selectedUnit != null){
			dmg = TouchHandler.selectedUnit.damage; 
			hp = TouchHandler.selectedUnit.hp;
		}

		backTexture = (Texture2D)Resources.Load<Texture2D>("Mekkano Assets/Textures/backplate");
		phaseTexture = (Texture2D)Resources.Load<Texture2D>(loadTexture);
		
		player = Player.playerIndex;
		
		rotAngle = (player==1?0:180);
		pivotPoint = new Vector2(Screen.width/2, Screen.height/2);
		GUIUtility.RotateAroundPivot(rotAngle, pivotPoint);

		GUI.DrawTexture(new Rect(0, Screen.height-screenHScale, Screen.width, Screen.height/10),backTexture);
		GUI.DrawTexture(new Rect(screenWScale, Screen.height-screenHScale, screenWScale, screenHScale), phaseTexture);
		GUI.TextArea(new Rect(Screen.width-screenWScale, Screen.height-screenHScale, screenWScale, screenHScale),"dmg: " + dmg + "\n hp:" + hp);
		
		switch(Phases.phase){
		case 1:
			loadTexture = "Mekkano Assets/Textures/skor2";
			if (GUI.Button (new Rect (0, Screen.height-screenHScale, screenWScale, screenHScale), buttonText))
				Phases.selectPhase(Phases.phase+1);
			break;
		case 2:
			loadTexture = "Mekkano Assets/Textures/svard_icon";
			if (GUI.Button (new Rect (0, Screen.height-screenHScale, screenWScale, screenHScale), buttonText))
				Phases.selectPhase(Phases.phase+1);
			break;
		case 3:
			loadTexture = "Mekkano Assets/Textures/sword_shield";
			if (GUI.Button (new Rect (0, Screen.height-screenHScale, screenWScale, screenHScale), buttonText)){
				Phases.selectPhase(Phases.phase-2);
			}
			break;
		}
	}
}
