using UnityEngine;
using System.Collections.Generic;
using System.Linq;

public class MatchesInfo {

	public BonusType bonusesContained { get; set; }

	private List<GameObject> matches;

	public MatchesInfo() {
		matches = new List<GameObject>();
		bonusesContained = BonusType.None;
	}

	public IEnumerable<GameObject> MatchedCandy {
		get {
			return matches.Distinct ();
		}
	}

	public void AddObject (GameObject go) {

		if (!matches.Contains (go)) {
			matches.Add (go);
		}

	}

	public void AddObjectRange (IEnumerable<GameObject> gos) {

		foreach (GameObject item in gos) {
			AddObject (item);
		}

	}

}
