using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Clipboarder : MonoBehaviour {
	public GameObject Clips;
	public int Delay;

	private float NextTime = -20;

	void Start() {
		AudioSource[] Sass = Clips.GetComponentsInChildren<AudioSource> ();
		AudioSource temp = Sass[Random.Range(0, Sass.Length)];
		temp.Play ();
	}
}
