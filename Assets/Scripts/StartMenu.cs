using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class StartMenu : MonoBehaviour {

	// Use this for initialization
	void Start () {
	}
	
	// Update is called once per frame
	void Update () {
		
	}

	public void StartPressed() {
		SceneManager.LoadScene ("LevelOne");
	}

	public void QuitPressed() {
		Application.Quit ();
	}
}
