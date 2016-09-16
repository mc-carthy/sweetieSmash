using UnityEngine;
using System.Collections;

public class SoundManager : MonoBehaviour {

	private AudioSource source;

	private void Awake () {
		source = GetComponent<AudioSource> ();
	}

	public void PlaySound () {
		source.Play ();
	}
	
}
