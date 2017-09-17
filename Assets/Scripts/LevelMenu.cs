using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class LevelMenu : MonoBehaviour {
	private GameObject Menu;
	private bool Paused;

	// Use this for initialization
	void Start () {
		// Sets up menu
		Menu = GameObject.FindGameObjectWithTag ("Menu");
		Menu.SetActive (false);
		Paused = false;
	}
	
	// Update is called once per frame
	void Update () {
		if (Input.GetKeyDown (KeyCode.Escape)) {
			Menu.SetActive (true);
			Paused = true;
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
