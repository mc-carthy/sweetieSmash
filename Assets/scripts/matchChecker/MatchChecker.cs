using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class MatchChecker {

	public static IEnumerator AnimatePotentialMatches (IEnumerable<GameObject> potentialMatches) {

		for (float i = 1.0f; i >= 0.3f; i -= 0.1f) {
			foreach (GameObject item in potentialMatches) {
				Color c = item.GetComponent<SpriteRenderer> ().color;
				c.a = i;
				item.GetComponent<SpriteRenderer> ().color = c;
			}
			yield return new WaitForSeconds (GameVariables.opacityAnimationDelay);
		}

		for (float i = 0.3f; i <= 1.0f; i += 0.1f) {
			foreach (GameObject item in potentialMatches) {
				Color c = item.GetComponent<SpriteRenderer> ().color;
				c.a = i;
				item.GetComponent<SpriteRenderer> ().color = c;
			}
			yield return new WaitForSeconds (GameVariables.opacityAnimationDelay);
		}
	}

	public static bool AreHorizontalOrVerticalNeighbours (Candy c0, Candy c1) {
		return (c0.column == c1.column || c0.row == c1.row) && Mathf.Abs (c0.column - c1.column) <= 1 && Mathf.Abs (c0.row - c1.row) <= 1;
	}

	public static IEnumerable<GameObject> GetPotentialMatches (CandyArray candies) {
		List<List<GameObject>> matches = new List<List<GameObject>> ();



		return null;
	}

	public static List<GameObject> CheckHorizontal0 (int row, int column, CandyArray candies) {

		if (column <= GameVariables.columns - 2) {

			if (candies [row, column].GetComponent<Candy> ().IsSameType (candies [row, column + 1].GetComponent<Candy>())) {

				if (row >= 1 && column >= 1) {

					if (candies [row, column].GetComponent<Candy> ().IsSameType (candies [row - 1, column - 1].GetComponent<Candy> ())) {
						return new List<GameObject> {
							candies [row, column],
							candies [row, column + 1],
							candies [row - 1, column - 1]
						};
					}

					if (candies [row, column].GetComponent<Candy> ().IsSameType (candies [row + 1, column - 1].GetComponent<Candy> ())) {
						return new List<GameObject> {
							candies [row, column],
							candies [row, column + 1],
							candies [row + 1, column - 1]
						};
					}

				}

			}

		}

		return null;

	}

	public static List<GameObject> CheckHorizontal1 (int row, int column, CandyArray candies) {

		if (column <= GameVariables.columns - 3) {

			if (candies [row, column].GetComponent<Candy> ().IsSameType (candies [row, column + 1].GetComponent<Candy>())) {

				if (row >= 1 && column <= GameVariables.columns - 3) {

					if (candies [row, column].GetComponent<Candy> ().IsSameType (candies [row - 1, column + 2].GetComponent<Candy> ())) {
						return new List<GameObject> {
							candies [row, column],
							candies [row, column + 1],
							candies [row - 1, column + 2]
						};
					}

					if (row <= GameVariables.rows - 2 && column <= GameVariables.columns - 3) {

						if (candies [row, column].GetComponent<Candy> ().IsSameType (candies [row + 1, column + 2].GetComponent<Candy> ())) {
							return new List<GameObject> {
								candies [row, column],
								candies [row, column + 1],
								candies [row + 1, column + 2]
							};
						}

					}

				}

			}

		}

		return null;

	}

	public static List<GameObject> CheckHorizontal2 (int row, int column, CandyArray candies) {

		if (column <= GameVariables.columns - 4) {

			if (
				candies [row, column].GetComponent<Candy> ().IsSameType (candies [row, column + 1].GetComponent<Candy>()) &&
				candies [row, column].GetComponent<Candy> ().IsSameType (candies [row, column + 3].GetComponent<Candy>())
			) {
				
				return new List<GameObject> {
					candies [row, column],
					candies [row, column + 1],
					candies [row, column + 3]
				};

			}

		}

		if (column >= 2 && column <= GameVariables.columns - 2) {

			if (
				candies [row, column].GetComponent<Candy> ().IsSameType (candies [row, column + 1].GetComponent<Candy>()) &&
				candies [row, column].GetComponent<Candy> ().IsSameType (candies [row, column - 2].GetComponent<Candy>())
			) {

				return new List<GameObject> {
					candies [row, column],
					candies [row, column + 1],
					candies [row, column - 2]
				};

			}

		}

		return null;

	}
}