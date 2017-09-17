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
		if (SceneManager.GetActiveScene ().name == "LevelTwoSuccess") {
			Application.OpenURL ("https://www.youtube.com/watch?v=dQw4w9WgXcQ");
		} else {
			SceneManager.LoadScene (NextScene);
		}
	}

	public void QuitPressed() {
		StartSound.Play ();
		SceneManager.LoadScene ("StartMenu");
	}
}
