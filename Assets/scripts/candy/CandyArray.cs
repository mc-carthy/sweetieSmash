using UnityEngine;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

public class CandyArray {

	private GameObject[,] candies = new GameObject[GameVariables.rows, GameVariables.columns];
	private GameObject backup0, backup1;

	public GameObject this[int row, int column] {
		get {
			try {
				return candies[row, column];
			} catch (Exception e) {
				throw;
			}
		}
		set {
			candies [row, column] = value;
		}
	}

	public void Swap (GameObject g0, GameObject g1) {

		backup0 = g0;
		backup1 = g1;

		Candy g0Candy = g0.GetComponent<Candy> ();
		Candy g1Candy = g1.GetComponent<Candy> ();

		int g0Row = g0Candy.row;
		int g1Row = g1Candy.row;		
		int g0Column = g0Candy.column;
		int g1Column = g1Candy.column;

		GameObject temp = candies [g0Row, g0Column];
		candies [g0Row, g0Column] = candies [g1Row, g1Column];
		candies [g1Row, g1Column] = temp;

		Candy.SwapRowColumn (g0Candy, g1Candy);

	}

	public void UndoSwap () {
		Swap (backup0, backup1);
	}

}
