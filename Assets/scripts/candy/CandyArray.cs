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

	private IEnumerable<GameObject> GetMatchesHorizontally (GameObject go) {
		List<GameObject> matches = new List<GameObject> ();
		matches.Add (go);

		Candy candy = go.GetComponent<Candy> ();

		// Search for matches to the left
		if (candy.column != 0) {
			for (int column = candy.column - 1; column >= 0; column--) {
				if (candies [candy.row, column].GetComponent<Candy> ().IsSameType (candy)) {
					matches.Add (candies [candy.row, column]);
				} else {
					break;
				}
			}
		}

		// Search for matches to the right
		if (candy.column != GameVariables.columns - 1) {
			for (int column = candy.column + 1; column < GameVariables.columns; column++) {
				if (candies [candy.row, column].GetComponent<Candy> ().IsSameType (candy)) {
					matches.Add (candies [candy.row, column]);
				} else {
					break;
				}
			}
		}

		if (matches.Count < GameVariables.minumumMatches) {
			matches.Clear ();
		}

		return matches.Distinct ();
	}

	private IEnumerable<GameObject> GetMatchesVertically (GameObject go) {
		List<GameObject> matches = new List<GameObject> ();
		matches.Add (go);

		Candy candy = go.GetComponent<Candy> ();

		// Search for matches below
		if (candy.row != 0) {
			for (int row = candy.row - 1; row >= 0; row--) {
				if (candies [row, candy.column].GetComponent<Candy> ().IsSameType (candy)) {
					matches.Add (candies [row, candy.column]);
				} else {
					break;
				}
			}
		}

		// Search for matches above
		if (candy.row != GameVariables.rows - 1) {
			for (int row = candy.row + 1; row < GameVariables.rows; row++) {
				if (candies [row, candy.column].GetComponent<Candy> ().IsSameType (candy)) {
					matches.Add (candies [row, candy.column]);
				} else {
					break;
				}
			}
		}

		if (matches.Count < GameVariables.minumumMatches) {
			matches.Clear ();
		}

		return matches.Distinct ();
	}

	private bool ContainsDestroyWholeRowColumnBonus (IEnumerable<GameObject> matches) {
		if (matches.Count() >= GameVariables.minumumMatches) {
			
			foreach (GameObject item in matches) {
				if (BonusTypeChecker.ContainsDestroyWholeRowColumn (item.GetComponent<Candy> ().bonus)) {
					return true;
				}
			}

		}

		return false;
	}

	private IEnumerable<GameObject> GetEntireRow (GameObject go) {
		List<GameObject> matches = new List<GameObject> ();

		int row = go.GetComponent<Candy> ().row;

		for (int column = 0; column < GameVariables.columns; column++) {
			matches.Add (candies [row, column]);
		}

		return matches;
	}

	private IEnumerable<GameObject> GetEntireColumn (GameObject go) {
		List<GameObject> matches = new List<GameObject> ();

		int column = go.GetComponent<Candy> ().column;

		for (int row = 0; row < GameVariables.rows; row++) {
			matches.Add (candies [row, column]);
		}

		return matches;
	}

	public void Remove (GameObject item) {
		candies [item.GetComponent<Candy> ().row, item.GetComponent<Candy> ().column] = null;
	}

	public AlteredCandyInfo Collapse (IEnumerable<int> columns) {

		AlteredCandyInfo collapseInfo = new AlteredCandyInfo ();

		foreach (int column in columns) {
			for (int row0 = 0; row0 < GameVariables.rows - 1; row0++) {
				if (candies [row0, column] == null) {

					for (int row1 = row0 + 1; row1 < GameVariables.rows; row1++) {

						if (candies [row1, column] != null) {
							candies [row0, column] = candies [row1, column];
							candies [row1, column] = null;

							if (row1 - row0 > collapseInfo.maxDistance) {

								collapseInfo.maxDistance = row1 - row0;

							}

							candies [row0, column].GetComponent<Candy> ().row = row0;
							candies [row0, column].GetComponent<Candy> ().column= column;

							collapseInfo.AddCandy (candies [row0, column]);
							break;

						}

					}

				}
			}
		}

		return collapseInfo;

	}

	public IEnumerable<CandyInfo> GetEmptyItemsOnColumn (int column) {

		List<CandyInfo> emptyItems = new List<CandyInfo> ();

		for (int row = 0; row < GameVariables.rows; row++) {

			if (candies [row, column] == null) {
				emptyItems.Add (new CandyInfo() { _row = row, _column = column });
			}

		}

		return emptyItems;

	}

}
