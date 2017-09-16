using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class LevelMenu : MonoBehaviour {
	public AudioSource SuccessSound;
	public AudioSource FailureSound;

	private GameObject Menu;
	private bool Paused;
	private 

	// Use this for initialization
	void Start () {
		// Sets up menu
		Menu = GameObject.FindGameObjectWithTag ("Menu");
		Menu.SetActive (false);
		Paused = false;

		// Lets sounds play over scene transition
		DontDestroyOnLoad (SuccessSound);
		DontDestroyOnLoad (FailureSound);
	}
	
	// Update is called once per frame
	void Update () {
		if (Input.GetKeyDown (KeyCode.Escape)) {
			Menu.SetActive (true);
			Paused = true;
		}
		if (Input.GetKeyDown (KeyCode.Space)) {
			SuccessSound.Play ();
			SceneManager.LoadScene ("LevelExitSuccess");
		}
		if (Input.GetKeyDown (KeyCode.LeftShift)) {
			FailureSound.Play ();
			SceneManager.LoadScene ("LevelExitFailure");
		}
	}

	public void ResumePressed () {
		Menu.SetActive (false);
		Paused = false;
	}

	public void RestartPressed() {
		string name = SceneManager.GetActiveScene().name;
		SceneManager.LoadScene (name);
	}

	public void QuitPressed() {
		SceneManager.LoadScene ("StartMenu");
	}
}
