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
	public GameObject[] candyPrefabs;
	public GameObject[] explosionPrefabs;
	public GameObject[] bonusPrefabs;
	private IEnumerator checkPotentialMatchesCoroutine;
	private IEnumerator animatePotentialMatchesCoroutine;
	private IEnumerable<GameObject> potentialMatches;

	private void Start () {
		InitializeTypesOnPrefabShapesAndBonuses ();
		InitializeCandyAndSpawnPositions ();
	}

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

	private void InitializeTypesOnPrefabShapesAndBonuses () {

		foreach (GameObject item in candyPrefabs) {
			item.GetComponent<Candy> ().type = item.name;
		}

		for (int i = 0; i < bonusPrefabs.Length; i++) {
			bonusPrefabs [i].GetComponent<Candy> ().type = candyPrefabs [i].name;
		}

	}

	private void InstantiateAndPlaceNewCandy (int row, int column, GameObject newCandy) {

		GameObject go = Instantiate(newCandy, bottomRight + new Vector2 (column * candySize.x, row * candySize.y), Quaternion.identity) as GameObject;

		go.GetComponent<Candy> ().Initialize (newCandy.GetComponent<Candy> ().type, row, column);

		candies [row, column] = go;

	}

	private void SetUpSpawnPositions () {

		for (int column = 0; column < GameVariables.columns; column++) {
			spawnPositions [column] = bottomRight + new Vector2 (column * candySize.x, GameVariables.rows * candySize.y);
		}

	}

	private GameObject GetRandomCandy () {
		return candyPrefabs [Random.Range (0, candyPrefabs.Length)];
	}

	private GameObject GetRandomExplosion () {
		return explosionPrefabs [Random.Range (0, explosionPrefabs.Length)];
	}

	public void InitializeCandyAndSpawnPositions () {
		InitializeVariables ();

		if (candies != null) {
			DestroyAllCandy ();
		}

		candies = new CandyArray ();

		spawnPositions = new Vector2 [GameVariables.columns];

		for (int row = 0; row < GameVariables.rows; row++) {
			for (int column = 0; column < GameVariables.columns; column++) {
				GameObject newCandy = GetRandomCandy ();

				while (
					column >= 2 && 
					candies [row, column - 1].GetComponent<Candy> ().IsSameType (newCandy.GetComponent<Candy> ()) && 
					candies [row, column - 2].GetComponent<Candy> ().IsSameType (newCandy.GetComponent<Candy> ())) 
				{
					newCandy = GetRandomCandy ();

				}

				while (
					row >= 2 && 
					candies [row - 1, column].GetComponent<Candy> ().IsSameType (newCandy.GetComponent<Candy> ()) && 
					candies [row - 2, column].GetComponent<Candy> ().IsSameType (newCandy.GetComponent<Candy> ())) 
				{
					newCandy = GetRandomCandy ();

				}

				InstantiateAndPlaceNewCandy (row, column, newCandy);

			}
		}

		SetUpSpawnPositions ();

	}

	private void StartCheckForPotentialMatches () {
		StopCheckForPotentialMatches ();

		checkPotentialMatchesCoroutine = CheckForPotentialMatches ();
		StartCoroutine (checkPotentialMatchesCoroutine);
	}

	private void StopCheckForPotentialMatches () {

		if (animatePotentialMatchesCoroutine != null) {
			StopCoroutine (animatePotentialMatchesCoroutine);
		}

		if (checkPotentialMatchesCoroutine != null) {
			StopCoroutine (checkPotentialMatchesCoroutine);
		}

		ResetOpacityOnPotentialMatches ();

	}

	private void ResetOpacityOnPotentialMatches () {

		if (potentialMatches != null) {

			foreach (GameObject item in potentialMatches) {
				if (item != null) {
					Color c = item.GetComponent<SpriteRenderer> ().color;
					c.a = 1;
					item.GetComponent<SpriteRenderer> ().color = c;
				}
			}

		}

	}

	private IEnumerator CheckForPotentialMatches () {
		yield return new WaitForSeconds (GameVariables.waitBeforePotentialMatchesCheck);

		potentialMatches = MatchChecker.GetPotentialMatches (candies);

		if (potentialMatches != null) {

			while (true) {
				animatePotentialMatchesCoroutine = MatchChecker.AnimatePotentialMatches(potentialMatches);
				StartCoroutine (animatePotentialMatchesCoroutine);
				yield return new WaitForSeconds (GameVariables.waitBeforePotentialMatchesCheck);
			}

		}
	}

}
