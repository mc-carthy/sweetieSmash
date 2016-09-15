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
			yield return new WaitForSeconds (GameVariables.OpacityAnimationDelay);
		}

		for (float i = 0.3f; i <= 1.0f; i += 0.1f) {
			foreach (GameObject item in potentialMatches) {
				Color c = item.GetComponent<SpriteRenderer> ().color;
				c.a = i;
				item.GetComponent<SpriteRenderer> ().color = c;
			}
			yield return new WaitForSeconds (GameVariables.OpacityAnimationDelay);
		}
	}

	public static bool AreHorizontalOrVerticalNeighbours (Candy c0, Candy c1) {
		return (c0.column == c1.column || c0.row == c1.row) && Mathf.Abs(c0.column - c1.column) <= 1 && Mathf.Abs(c0.row - c1.row) <= 1
	}
}