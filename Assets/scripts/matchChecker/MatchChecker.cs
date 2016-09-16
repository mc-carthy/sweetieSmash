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

		for (int row = 0; row < GameVariables.rows; row++) {
			for (int column = 0; column < GameVariables.columns; column++) {
				List<GameObject> matches0 = CheckHorizontal0 (row, column, candies);
				List<GameObject> matches1 = CheckHorizontal1 (row, column, candies);
				List<GameObject> matches2 = CheckHorizontal2 (row, column, candies);				
				List<GameObject> matches3 = CheckVertical0 (row, column, candies);
				List<GameObject> matches4 = CheckVertical1 (row, column, candies);
				List<GameObject> matches5 = CheckVertical2 (row, column, candies);

				if (matches0 != null) {	matches.Add (matches0); }
				if (matches1 != null) {	matches.Add (matches1); }
				if (matches2 != null) {	matches.Add (matches2); }
				if (matches3 != null) {	matches.Add (matches3); }
				if (matches4 != null) {	matches.Add (matches4); }
				if (matches5 != null) {	matches.Add (matches5); }

				if (matches.Count >= 3) {
					return matches[Random.Range(0, matches.Count - 1)];
				}

				if (row >= GameVariables.rows / 2 && matches.Count > 0 && matches.Count <= 2) {
					return matches[Random.Range(0, matches.Count - 1)];
				}
			}
		}

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

	public static List<GameObject> CheckVertical0 (int row, int column, CandyArray candies) {

		if (row <= GameVariables.rows - 2) {

			if (candies [row, column].GetComponent<Candy> ().IsSameType (candies [row + 1, column].GetComponent<Candy>())) {

				if (column >= 1 && row >= 1) {
					if (candies [row, column].GetComponent<Candy> ().IsSameType (candies [row - 1, column - 1].GetComponent<Candy> ())) {
						return new List<GameObject> {
							candies [row, column],
							candies [row + 1, column],
							candies [row - 1, column - 1]
						};
					}
				}

				if (column <= GameVariables.columns - 2 && row >= 1) {
					if (candies [row, column].GetComponent<Candy> ().IsSameType (candies [row - 1, column + 1].GetComponent<Candy> ())) {
						return new List<GameObject> {
							candies [row, column],
							candies [row + 1, column],
							candies [row - 1, column + 1]
						};
					}
				}

			}

		}

		return null;

	}

	public static List<GameObject> CheckVertical1 (int row, int column, CandyArray candies) {

		if (row <= GameVariables.rows - 3) {

			if (candies [row, column].GetComponent<Candy> ().IsSameType (candies [row + 1, column].GetComponent<Candy>())) {

				if (column >= 1) {
					if (candies [row, column].GetComponent<Candy> ().IsSameType (candies [row + 2, column - 1].GetComponent<Candy> ())) {
						return new List<GameObject> {
							candies [row, column],
							candies [row + 1, column],
							candies [row + 2, column - 1]
						};
					}
				}

				if (column <= GameVariables.columns - 2) {
					if (candies [row, column].GetComponent<Candy> ().IsSameType (candies [row + 2, column + 1].GetComponent<Candy> ())) {
						return new List<GameObject> {
							candies [row, column],
							candies [row + 1, column],
							candies [row + 2, column + 1]
						};
					}
				}

			}

		}

		return null;

	}

	public static List<GameObject> CheckVertical2 (int row, int column, CandyArray candies) {

		if (row <= GameVariables.rows - 4) {

			if (
				candies [row, column].GetComponent<Candy> ().IsSameType (candies [row + 1, column].GetComponent<Candy>()) &&
				candies [row, column].GetComponent<Candy> ().IsSameType (candies [row + 3, column].GetComponent<Candy>())
			) {

				return new List<GameObject> {
					candies [row, column],
					candies [row + 1, column],
					candies [row + 3, column]
				};

			}

		}

		if (row >= 2 && row <= GameVariables.rows - 2) {

			if (
				candies [row, column].GetComponent<Candy> ().IsSameType (candies [row + 1, column].GetComponent<Candy>()) &&
				candies [row, column].GetComponent<Candy> ().IsSameType (candies [row - 2, column].GetComponent<Candy>())
			) {

				return new List<GameObject> {
					candies [row, column],
					candies [row + 1, column],
					candies [row - 2, column]
				};

			}

		}

		return null;
	}
}