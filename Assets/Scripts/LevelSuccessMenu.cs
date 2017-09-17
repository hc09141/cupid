using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class LevelSuccessMenu : MonoBehaviour {
	public string NextScene;
	public AudioSource StartSound;

	void Awake() {
		DontDestroyOnLoad (StartSound);
	}

	public void StartPressed() {
		StartSound.Play ();
		SceneManager.LoadScene (NextScene);
	}

	public void QuitPressed() {
		StartSound.Play ();
		SceneManager.LoadScene ("StartMenu");
	}
}
