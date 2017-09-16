using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class LevelStart : MonoBehaviour {
	public AudioSource StartSound;

	void Awake() {
		DontDestroyOnLoad (StartSound);
	}

	public void StartPressed() {
		StartSound.Play ();
		SceneManager.LoadScene ("LevelOne");
	}
}
