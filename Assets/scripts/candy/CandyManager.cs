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
	private GameObject hitGo0 = null;
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
		StartCheckForPotentialMatches ();
	}

	private void Update () {

		if (state == GameState.None) {
			if (Input.GetMouseButtonDown (0)) {
				RaycastHit2D hit = Physics2D.Raycast (Camera.main.ScreenToWorldPoint (Input.mousePosition), Vector2.zero);

				if (hit.collider != null) {
					hitGo0 = hit.collider.gameObject;
					state = GameState.SelectionStarted;
				}
			}
		} else if (state == GameState.SelectionStarted) {
			if (Input.GetMouseButtonDown (0)) {
				RaycastHit2D hit = Physics2D.Raycast (Camera.main.ScreenToWorldPoint (Input.mousePosition), Vector2.zero);

				if (hit.collider != null && hitGo0 != hit.collider.gameObject) {
					StopCheckForPotentialMatches ();

					if (MatchChecker.AreHorizontalOrVerticalNeighbours (hitGo0.GetComponent<Candy> (), hit.collider.gameObject.GetComponent<Candy> ())) {

						state = GameState.Animating;
						FixSortingLayer (hitGo0, hit.collider.gameObject);
						StartCoroutine (FindMatchesAndCollapse (hit));

					} else {
						state = GameState.None;
					}
				}
			}
		}

	}

	private void InitializeVariables () {
		score = 0;
		ShowScore ();
	}

	private void ShowScore () {
		scoreText.text = "Score: " + score.ToString ();
	}

	private void IncreaseScore (int value) {
		score += value;
		ShowScore ();
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

	private void FixSortingLayer (GameObject hitGo0, GameObject hitGo1) {

		SpriteRenderer sp0 = hitGo0.GetComponent<SpriteRenderer> ();
		SpriteRenderer sp1 = hitGo1.GetComponent<SpriteRenderer> ();

		if (sp0.sortingOrder <= sp1.sortingOrder) {
			sp0.sortingOrder = 1;
			sp1.sortingOrder = 0;
		}

	}

	private void RemoveFromScene (GameObject item) {

		GameObject explosion = Instantiate (GetRandomExplosion (), item.transform.position, Quaternion.identity) as GameObject;
		Destroy (explosion, GameVariables.explosionAnimationDuration);
		Destroy (item);

	}

	private GameObject GetBonusFromType (string type) {
		string color = type.Split ('_') [1].Trim ();

		foreach (GameObject item in bonusPrefabs) {
			if (item.GetComponent<Candy> ().type.Contains (color)) {
				return item;
			}
		} throw new System.Exception ("You passed the wrong type");
	}

	private void CreateBonus (Candy hitGoCache) {
		GameObject bonus = Instantiate (
			GetBonusFromType(hitGoCache.type), 
			bottomRight + new Vector2(hitGoCache.column * candySize.x, hitGoCache.row * candySize.y), 
			Quaternion.identity) as GameObject;

		candies [hitGoCache.row, hitGoCache.column] = bonus;

		Candy bonusCandy = bonus.GetComponent<Candy> ();
		bonusCandy.Initialize (hitGoCache.type, hitGoCache.row, hitGoCache.column);
		bonusCandy.bonus = BonusType.DestroyWholeRowColumn;
	}

	private AlteredCandyInfo CreateNewCandyInSpecificColumns (IEnumerable<int> columnsWithMissingCandies) {

		AlteredCandyInfo newCandyInfo = new AlteredCandyInfo ();

		foreach (int column in columnsWithMissingCandies) {

			IEnumerable<CandyInfo> emptyItems = candies.GetEmptyItemsOnColumn (column);

			// Double check if type is Candy
			foreach (CandyInfo item in emptyItems) {

				GameObject go = GetRandomCandy ();
				GameObject newCandy = Instantiate (go, spawnPositions [column], Quaternion.identity) as GameObject;
				newCandy.GetComponent<Candy> ().Initialize (go.GetComponent<Candy> ().type, item._row, item._column);

				if (GameVariables.rows - item._row > newCandyInfo.maxDistance) {
					newCandyInfo.maxDistance = GameVariables.rows - item._row;
				}

				candies [item._row, item._column] = newCandy;
				newCandyInfo.AddCandy (newCandy);

			}

		}

		return newCandyInfo;

	}

	private void MoveAndAnimate (IEnumerable<GameObject> movedGameObjects, int distance) {

		foreach (GameObject item in movedGameObjects) {
			item.transform.positionTo (
				GameVariables.moveAnimationDuration * distance, 
				bottomRight + new Vector2(item.GetComponent<Candy>().column * candySize.x, item.GetComponent<Candy>().row * candySize.y)
			);
		}
	}

	private IEnumerator FindMatchesAndCollapse (RaycastHit2D hit1) {
		GameObject hitGo1 = hit1.collider.gameObject;
		candies.Swap (hitGo0, hitGo1);

		hitGo0.transform.positionTo (GameVariables.animationDuration, hit1.transform.position);
		hitGo1.transform.positionTo (GameVariables.animationDuration, hitGo0.transform.position);
		yield return new WaitForSeconds (GameVariables.animationDuration);

		MatchesInfo hitGo0MatchesInfo = candies.GetMatches (hitGo0);
		MatchesInfo hitGo1MatchesInfo = candies.GetMatches (hitGo1);

		IEnumerable<GameObject> totalMatches = hitGo0MatchesInfo.MatchedCandy.Union (hitGo1MatchesInfo.MatchedCandy).Distinct ();

		if (totalMatches.Count () < GameVariables.minumumMatches) {

			hitGo0.transform.positionTo (GameVariables.animationDuration, hitGo1.transform.position);
			hitGo1.transform.localPositionTo (GameVariables.animationDuration, hitGo0.transform.position);
			yield return new WaitForSeconds (GameVariables.animationDuration);

			candies.UndoSwap ();
		}

		bool addBonus = 
			totalMatches.Count () >= GameVariables.minumumMatchesForBonus &&
			!BonusTypeChecker.ContainsDestroyWholeRowColumn (hitGo0MatchesInfo.bonusesContained) &&
			!BonusTypeChecker.ContainsDestroyWholeRowColumn (hitGo1MatchesInfo.bonusesContained);

		Candy hitGo0Cache = null;

		if (addBonus) {
			hitGo0Cache = new Candy ();
			GameObject sameTypeGo = hitGo0MatchesInfo.MatchedCandy.Count () > 0 ? hitGo0 : hitGo1;
			Candy candy = sameTypeGo.GetComponent<Candy> ();

			hitGo0Cache.Initialize (candy.type, candy.row, candy.column);
		}
			
		int timesRun = 1;

		while (totalMatches.Count () >= GameVariables.minumumMatches) {

			IncreaseScore((totalMatches.Count () - 2) * GameVariables.match3Score);
			print ((totalMatches.Count () - 2) * GameVariables.match3Score);
			if (timesRun >= 2) {
				IncreaseScore (GameVariables.subsequelMatchScore);
			}
			soundManager.PlaySound ();

			foreach (GameObject item in totalMatches) {
				candies.Remove (item);
				RemoveFromScene (item);
			}

			if (addBonus) {
				CreateBonus (hitGo0Cache);
			}

			addBonus = false;

			IEnumerable<int> columns = totalMatches.Select (go => go.GetComponent<Candy> ().column).Distinct ();

			AlteredCandyInfo collapsedCandyInfo = candies.Collapse (columns);

			AlteredCandyInfo newCandyInfo = CreateNewCandyInSpecificColumns (columns);

			int maxDistance = Mathf.Max (collapsedCandyInfo.maxDistance, newCandyInfo.maxDistance);

			MoveAndAnimate (newCandyInfo.AlteredCandy, maxDistance);
			MoveAndAnimate (collapsedCandyInfo.AlteredCandy, maxDistance);

			yield return new WaitForSeconds (GameVariables.moveAnimationDuration * maxDistance);

			totalMatches = candies.GetMatches (collapsedCandyInfo.AlteredCandy).Union (candies.GetMatches (newCandyInfo.AlteredCandy)).Distinct ();

			timesRun++;

		}

		state = GameState.None;
		StartCheckForPotentialMatches ();
	}
}
