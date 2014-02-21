using UnityEngine;
using System.Collections;

public class Main : MonoBehaviour {
	private Grid grid;

	void Start () {
		grid = gameObject.AddComponent<Grid>();
		grid.createGrid (0, 0, 10, 15, 1);
	}

	void Update () {
	
	}
}
