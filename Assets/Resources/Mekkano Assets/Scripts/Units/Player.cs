﻿using UnityEngine;
using System;
using System.Collections;
using System.Linq;

public class Player : MonoBehaviour {

	public int unitsAmount, lines, lineStart, movementPointBase = 4;
	public Units[,] units;
	public static int playerIndex;
	public int movementPoints;
	private int gridXlength = Main.gridXlength, gridZlength = Main.gridZlength;
	private string unitType;

	public bool[] hasBuff = new bool[2]; //index: 0 = controlPointDmg, 1 = controlPointMove
	public bool damageBuff = false;
	public bool movementBuff = false;

	private string getUnitFile(){
		try{
			TextAsset unitPlacement = Resources.Load("Mekkano Assets/Scripts/Units/unitPlacement") as TextAsset;
			return unitPlacement.text.Replace(",","").Replace(" ","").Replace(Environment.NewLine,"");
		} catch(Exception e){
			Debug.Log("Error " + e + " occurred");	
		}
		return null;
	}

	void Awake(){
		movementPoints = movementPointBase;
		unitType = getUnitFile();
		unitsAmount = unitType.Length;
	}

	public void createUnits(Grid grid, float gridHeight, bool side){
		int direction = (side? 1:-1);

		if (side){
			lines = 0;  // the variable lines is added to check if it's player 1 or player 2 position, if it's player 2 it starts on the other edge of the grid
		} else {
			lines = gridXlength;
			lineStart = lines; // lineStart is always the same as line was before the loop started
		}
		
		units = new Units[gridZlength,gridXlength]; // creating the Units multiarray for the player class so we can find a specific unit in the class
		
		for(int i = 0; i < (unitsAmount > (gridXlength*gridZlength)?(gridXlength*gridZlength): unitsAmount); i++){ // the for loops never loops more than the amount of tile there is on the field

			int currentColumn = (int)(i/gridZlength);

			switch(unitType[i]){
				case '1': 			
						units[i-currentColumn*gridZlength,currentColumn]
						= gameObject.AddComponent<MeleeUnit>();
					break;
				case '2': 
						units[i-currentColumn*gridZlength,currentColumn]
						= gameObject.AddComponent<RangeUnit>();
					break;
				case '3': 
						units[i-currentColumn*gridZlength,currentColumn]
						= gameObject.AddComponent<SupportUnit>();
					break;
				case '4': 
						units[i-currentColumn*gridZlength,currentColumn]
						= gameObject.AddComponent<CommanderUnit>();
					break;
			}
			
			units[i-currentColumn*gridZlength,currentColumn].create   // placing the units on different locations depending on if it's player 1 or 2
				(grid.getTile((currentColumn*direction)+lineStart+(side?0:-1),
				              i-currentColumn*gridZlength), side);

			units[i-currentColumn*gridZlength,currentColumn].owner = this;

			if (side == false){
				lines = lineStart - currentColumn*2; // decreasing the lines depending on what column the unit is on (*2 because of currentColumn adds +1 column each "i" 1-2=-1 insead of 1-1=0 to make it decrease in columns each time it has looped each column)
			}
		}
	}
}
