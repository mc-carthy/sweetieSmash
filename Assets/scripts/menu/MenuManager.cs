using UnityEngine;
using UnityEngine.SceneManagement;
using System.Collections;

public class MenuManager : MonoBehaviour {

	public void LoadLevel () {
		SceneManager.LoadScene ("main", LoadSceneMode.Single);
	}

	public void GoToMenu () {
		SceneManager.LoadScene ("menu", LoadSceneMode.Single);
	}
}
