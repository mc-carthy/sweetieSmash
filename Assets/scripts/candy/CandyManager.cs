using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

public class CandyManager : MonoBehaviour {

	public Text scoreText;
	public SoundManager soundManager;
	public CandyArray candies;

	private int score;
	private Vector2 bottomRight = new Vector2 (-2.37f, -4.27f);
	private Vector2 candySize = new Vector2 (0.7f, 0.7f);
	private GameState state = GameState.None;
	private GameObject hitGo = null;
	private Vector2[] spawnPositions;
	private GameObject[] candyPrefabs;
	private GameObject[] explosionPrefabs;
	private GameObject[] bonusPrefabs;
	private IEnumerator checkPotentialMatchesCoroutine;
	private IEnumerator animatePotentialMatchesCoroutine;
	private IEnumerable<GameObject> potentialMatches;

	private void InitializeVariables () {
		score = 0;
		ShowScore ();
	}

	private void ShowScore () {
		scoreText.text = "Score: " + score;
	}

	private void IncreaseScore (int value) {
		score += value;
	}

	private void DestroyAllCandy () {
		for (int row = 0; row < GameVariables.rows; row++) {
			for (int column = 0; column < GameVariables.columns; column++) {
				Destroy (candies [row, column].gameObject);
			}
		}
	}

}
